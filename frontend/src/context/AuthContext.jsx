import { createContext, useState, useEffect } from "react";
import { login as loginApi } from "../api/authApi";

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const [user, setUser] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const token = localStorage.getItem("accessToken");
        if (token) {
            decodeAndSetUser(token);
        }
        setLoading(false);
    }, []);

    const decodeAndSetUser = (token) => {
        try {
            // Simple base64 decode for JWT payload to avoid extra deps like jwt-decode for now
            // A more robust app would use the library, but this works for standard JWTs
            const base64Url = token.split('.')[1];
            const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
            const jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
                return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
            }).join(''));

            const payload = JSON.parse(jsonPayload);

            // Map claims to user object
            // .NET Identity typically puts roles in specific claim types
            // http://schemas.microsoft.com/ws/2008/06/identity/claims/role
            const roleKey = Object.keys(payload).find(k => k.endsWith("/role")) || "role";

            setUser({
                email: payload.email || payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"],
                roles: Array.isArray(payload[roleKey]) ? payload[roleKey] : [payload[roleKey]],
                token: token,
            });
        } catch (error) {
            console.error("Failed to decode token", error);
            localStorage.removeItem("accessToken");
            setUser(null);
        }
    };

    const login = async (email, password) => {
        try {
            const data = await loginApi(email, password);
            if (data.accessToken) {
                localStorage.setItem("accessToken", data.accessToken);
                decodeAndSetUser(data.accessToken);
                return { success: true };
            }
        } catch (error) {
            console.error("AuthContext Login Error:", error);
            const msg = error.response?.data || error.message || "Login failed";
            return { success: false, error: msg };
        }
    };

    const logout = () => {
        localStorage.removeItem("accessToken");
        setUser(null);
    };

    return (
        <AuthContext.Provider value={{ user, login, logout, loading }}>
            {children}
        </AuthContext.Provider>
    );
};

export default AuthContext;
