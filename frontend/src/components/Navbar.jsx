import { Link, useNavigate } from "react-router-dom";
import { useContext } from "react";
import AuthContext from "../context/AuthContext";

export default function Navbar() {
    const { user, logout } = useContext(AuthContext);
    const navigate = useNavigate();

    const handleLogout = () => {
        logout();
        navigate("/login");
    };

    return (
        <nav className="navbar">
            <div className="container navbar-content">
                <Link to="/" className="logo">
                    Carpet<span className="accent">Cleaning</span>
                </Link>
                <div className="nav-links">
                    <Link to="/">Home</Link>

                    {user && (
                        <Link to="/dashboard">Dashboard</Link>
                    )}

                    {user && user.roles.includes("Admin") && (
                        <Link to="/admin/customers">Customers</Link>
                    )}

                    {!user ? (
                        <>
                            <Link to="/login" className="nav-btn">Login</Link>
                            <Link to="/register" className="nav-btn-primary">Register</Link>
                        </>
                    ) : (
                        <div style={{ display: 'flex', alignItems: 'center', gap: '0.8rem' }}>
                            <span style={{
                                fontSize: '0.85rem',
                                background: '#2563eb', // Vibrant blue
                                color: 'white',        // White text
                                padding: '0.4rem 0.8rem',
                                borderRadius: '99px',
                                fontWeight: '600',
                                boxShadow: '0 2px 4px rgba(37, 99, 235, 0.2)',
                                textTransform: 'uppercase',
                                letterSpacing: '0.5px'
                            }}>
                                👤 {user.roles?.[0] || 'User'}
                            </span>
                            <button onClick={handleLogout} className="nav-btn-outline">
                                Logout
                            </button>
                        </div>
                    )}
                </div>
            </div>
        </nav>
    );
}
