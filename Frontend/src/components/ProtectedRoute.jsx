import { useContext } from "react";
import { Navigate, Outlet } from "react-router-dom";
import AuthContext from "../context/AuthContext";

const ProtectedRoute = ({ allowedRoles }) => {
    const { user, loading } = useContext(AuthContext);

    if (loading) {
        return <div>Loading...</div>; // Or a nice spinner
    }

    if (!user) {
        return <Navigate to="/login" replace />;
    }

    // Check if user has at least one of the allowed roles
    // If allowedRoles is null/empty, just require login
    if (allowedRoles && allowedRoles.length > 0) {
        const hasRole = user.roles.some(role => allowedRoles.includes(role));
        if (!hasRole) {
            return <Navigate to="/" replace />; // Unauthorized: Go to home
        }
    }

    return <Outlet />;
};

export default ProtectedRoute;
