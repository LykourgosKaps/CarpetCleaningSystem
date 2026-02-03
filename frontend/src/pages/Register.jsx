import { useState, useContext } from "react";
import { useNavigate, Link } from "react-router-dom";
import AuthContext from "../context/AuthContext";
import client from "../api/client";

export default function Register() {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [error, setError] = useState("");
    const [loading, setLoading] = useState(false);

    const { login } = useContext(AuthContext);
    const navigate = useNavigate();

    async function handleSubmit(e) {
        e.preventDefault();
        setError("");

        if (password !== confirmPassword) {
            setError("Passwords do not match");
            return;
        }

        setLoading(true);
        try {
            // Direct call to register endpoint, then auto-login
            await client.post("/Auth/register", { email, password });

            // Auto-login
            const result = await login(email, password);
            if (result.success) {
                navigate("/dashboard");
            } else {
                navigate("/login");
            }
        } catch (err) {
            console.error("Registration error", err);
            const data = err.response?.data;
            // The API returns array of errors sometimes
            if (Array.isArray(data)) {
                setError(data.join(", "));
            } else {
                setError(typeof data === "string" ? data : "Registration failed. Please try again.");
            }
        } finally {
            setLoading(false);
        }
    }

    return (
        <div className="auth-form-container">
            <div className="form-header">
                <h2>Create Account</h2>
                <p>Join us for premium carpet care</p>
            </div>

            {error && <div className="error-msg">{error}</div>}

            <form onSubmit={handleSubmit}>
                <div className="form-group">
                    <label>Email Address</label>
                    <input
                        type="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        required
                        autoFocus
                    />
                </div>

                <div className="form-group">
                    <label>Password</label>
                    <input
                        type="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                        minLength={6}
                    />
                </div>

                <div className="form-group">
                    <label>Confirm Password</label>
                    <input
                        type="password"
                        value={confirmPassword}
                        onChange={(e) => setConfirmPassword(e.target.value)}
                        required
                    />
                </div>

                <button type="submit" className="btn btn-primary" style={{ width: "100%" }} disabled={loading}>
                    {loading ? "Creating Account..." : "Register"}
                </button>
            </form>

            <div style={{ marginTop: "1.5rem", textAlign: "center", fontSize: "0.9rem" }}>
                Already have an account? <Link to="/login" style={{ color: "var(--primary)", fontWeight: "600" }}>Login here</Link>
            </div>
        </div>
    );
}
