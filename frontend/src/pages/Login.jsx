import { useState, useContext } from "react";
import { useNavigate, Link } from "react-router-dom";
import AuthContext from "../context/AuthContext";

export default function Login() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");

  const { login } = useContext(AuthContext);
  const navigate = useNavigate();

  async function handleSubmit(e) {
    e.preventDefault();
    setError("");

    const result = await login(email, password);
    if (result.success) {
      navigate("/dashboard");
    } else {
      // DEBUGGING: Show exact error
      console.error("Login Result:", result);
      let errorMsg = "Login failed.";
      if (result.error) {
        if (typeof result.error === "string") errorMsg = result.error;
        else if (result.error.title) errorMsg = `${result.error.status}: ${result.error.title}`;
        else errorMsg = JSON.stringify(result.error);
      }
      setError(errorMsg);
    }
  }

  return (
    <div className="auth-form-container">
      <div className="form-header">
        <h2>Welcome Back</h2>
        <p>Sign in to your account</p>
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
          />
        </div>

        <button type="submit" className="btn btn-primary" style={{ width: "100%" }}>
          Sign In
        </button>
      </form>

      <div style={{ marginTop: "1.5rem", textAlign: "center", fontSize: "0.9rem" }}>
        Don't have an account? <Link to="/register" style={{ color: "var(--primary)", fontWeight: "600" }}>Register here</Link>
      </div>
    </div>
  );
}

