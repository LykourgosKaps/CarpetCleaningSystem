import { useState, useEffect, useContext } from "react";
import { useNavigate } from "react-router-dom";
import client from "../api/client";
import AuthContext from "../context/AuthContext";

export default function Dashboard() {
    const { user } = useContext(AuthContext);
    const [searchId, setSearchId] = useState("");
    const [searchPhone, setSearchPhone] = useState("");
    const [lookupResult, setLookupResult] = useState(null);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");

    // Admin Management State
    const [inProgressOrders, setInProgressOrders] = useState([]);
    const [completedOrders, setCompletedOrders] = useState([]);
    const [ordersLoading, setOrdersLoading] = useState(false);

    const navigate = useNavigate();
    const isAdmin = user?.roles?.includes("Admin");

    const fetchAdminOrders = async () => {
        if (!isAdmin) return;
        setOrdersLoading(true);
        try {
            const [ipRes, compRes] = await Promise.all([
                client.get("/Orders?status=2"), // IN_PROGRESS
                client.get("/Orders?status=3")  // COMPLETED
            ]);
            setInProgressOrders(ipRes.data.orders);
            setCompletedOrders(compRes.data.orders);
        } catch (err) {
            console.error("Failed to fetch admin orders", err);
        } finally {
            setOrdersLoading(false);
        }
    };

    useEffect(() => {
        fetchAdminOrders();
    }, [user, isAdmin]);

    const handleSearch = (e) => {
        e.preventDefault();
        if (searchId.trim()) {
            navigate(`/orders/${searchId}`);
        }
    };

    const handlePhoneLookup = async (e) => {
        e.preventDefault();
        const phone = searchPhone.trim();
        if (!phone) return;
        setLoading(true);
        setError("");
        setLookupResult(null);
        try {
            const res = await client.get(`/Customers/lookup?phone=${encodeURIComponent(phone)}`);
            setLookupResult(res.data);
        } catch (err) {
            console.error(err);
            const msg = err.response?.data?.detail || err.response?.data?.message || "No customer found with this phone number.";
            setError(msg);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="container" style={{ padding: "4rem 1rem" }}>
            <header style={{ marginBottom: "3rem", textAlign: "center" }}>
                <h1>Dashboard</h1>
                <p style={{ color: "var(--secondary)" }}>Manage your carpet cleaning orders</p>
                {error && <div className="error-msg" style={{ maxWidth: "400px", margin: "1rem auto" }}>{error}</div>}
            </header>

            <div className="features-grid" style={{ marginTop: "0" }}>

                {/* Search by Phone Card */}
                <div className="feature-card">
                    <div className="feature-icon">📱</div>
                    <h3>Lookup by Phone</h3>
                    <p>Find customer ID and latest order status.</p>

                    <form onSubmit={handlePhoneLookup} style={{ marginTop: "1rem" }}>
                        <div style={{ display: "flex", gap: "0.5rem" }}>
                            <input
                                type="text"
                                placeholder="Phone e.g. 69..."
                                value={searchPhone}
                                onChange={(e) => setSearchPhone(e.target.value)}
                                required
                                style={{ marginBottom: 0 }}
                            />
                            <button type="submit" className="btn btn-secondary" disabled={loading}>
                                {loading ? "..." : "Find"}
                            </button>
                        </div>
                    </form>

                    {lookupResult && (
                        <div style={{ marginTop: "1rem", padding: "1rem", background: "white", borderRadius: "8px", border: "1px solid #ddd", textAlign: "left" }}>
                            <div style={{ fontWeight: "bold", fontSize: "1.1rem" }}>{lookupResult.firstName} {lookupResult.lastName}</div>
                            <div style={{ fontSize: "0.9rem", color: "#666" }}>Customer ID: {lookupResult.customerId}</div>
                            {lookupResult.latestOrderId ? (
                                <div style={{ marginTop: "0.5rem" }}>
                                    <div style={{ fontSize: "0.85rem" }}>Latest Order: <strong>#{lookupResult.latestOrderId}</strong></div>
                                    <div style={{ fontSize: "0.85rem", color: "var(--primary)" }}>Status: {lookupResult.latestOrderStatus}</div>
                                    <button
                                        onClick={() => navigate(`/orders/${lookupResult.latestOrderId}`)}
                                        className="btn btn-primary"
                                        style={{ marginTop: "0.5rem", width: "100%", padding: "0.4rem" }}
                                    >
                                        View Order
                                    </button>
                                </div>
                            ) : (
                                <div style={{ marginTop: "0.5rem", fontSize: "0.85rem", fontStyle: "italic" }}>No orders found.</div>
                            )}
                            <button
                                onClick={() => navigate("/orders/new")}
                                className="btn btn-outline"
                                style={{ marginTop: "0.5rem", width: "100%", padding: "0.4rem" }}
                            >
                                Create New Order
                            </button>
                        </div>
                    )}
                </div>

                {/* Find Order Card */}
                <div className="feature-card">
                    <div className="feature-icon">🔍</div>
                    <h3>Find Order #</h3>
                    <p>Search for an existing order by ID.</p>

                    <form onSubmit={handleSearch} style={{ marginTop: "1rem" }}>
                        <div style={{ display: "flex", gap: "0.5rem" }}>
                            <input
                                type="number"
                                placeholder="Order ID"
                                value={searchId}
                                onChange={(e) => setSearchId(e.target.value)}
                                required
                                style={{ marginBottom: 0 }}
                            />
                            <button type="submit" className="btn btn-primary">Go</button>
                        </div>
                    </form>
                </div>

                {/* Create Order Card */}
                <div className="feature-card">
                    <div className="feature-icon">➕</div>
                    <h3>New Order</h3>
                    <p>Create a new carpet cleaning order.</p>
                    <button
                        onClick={() => navigate("/orders/new")}
                        className="btn btn-primary"
                        style={{ marginTop: "1rem", width: "100%" }}
                    >
                        Create Order
                    </button>
                </div>
            </div>

            {/* Admin Section */}
            {isAdmin && (
                <div style={{ marginTop: "4rem" }}>
                    <div style={{ background: "#f8fafc", padding: "2rem", borderRadius: "16px", border: "1px solid #e2e8f0", boxShadow: "inset 0 2px 4px 0 rgba(0, 0, 0, 0.05)" }}>
                        <h2 style={{ marginBottom: "2rem", color: "#1e293b", display: "flex", alignItems: "center", gap: "0.7rem", fontSize: "1.75rem" }}>
                            <span style={{
                                background: "#2563eb",
                                color: "white",
                                width: "40px",
                                height: "40px",
                                display: "flex",
                                alignItems: "center",
                                justifyContent: "center",
                                borderRadius: "10px",
                                fontSize: "1.2rem"
                            }}>🛡️</span>
                            Admin Management
                        </h2>

                        <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "2rem" }}>
                            {/* In Progress Box */}
                            <div style={{ background: "white", padding: "1.5rem", borderRadius: "12px", boxShadow: "0 4px 6px -1px rgba(0,0,0,0.1), 0 2px 4px -1px rgba(0,0,0,0.06)", border: "1px solid #f1f5f9" }}>
                                <h3 style={{ marginBottom: "1.5rem", color: "#2563eb", display: "flex", justifyContent: "space-between", alignItems: "center" }}>
                                    In Progress
                                    <span style={{ background: "#dbeafe", color: "#1e40af", padding: "0.2rem 0.8rem", borderRadius: "8px", fontSize: "0.85rem", fontWeight: "bold" }}>{inProgressOrders.length}</span>
                                </h3>
                                <div style={{ maxHeight: "400px", overflowY: "auto", paddingRight: "0.5rem" }}>
                                    {ordersLoading ? <p>Loading orders...</p> : inProgressOrders.length === 0 ? <p style={{ color: "#94a3b8", fontStyle: "italic" }}>No orders in progress.</p> : (
                                        inProgressOrders.map(o => (
                                            <div key={o.orderId} onClick={() => navigate(`/orders/${o.orderId}`)} style={{
                                                padding: "1rem",
                                                borderBottom: "1px solid #f1f5f9",
                                                cursor: "pointer",
                                                transition: "all 0.2s ease",
                                                borderRadius: "8px",
                                                marginBottom: "0.5rem"
                                            }} onMouseOver={e => e.currentTarget.style.background = "#f8fafc"} onMouseOut={e => e.currentTarget.style.background = "transparent"}>
                                                <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
                                                    <span style={{ fontWeight: "bold", fontSize: "1rem" }}>Order #{o.orderId}</span>
                                                    <span style={{ fontSize: "0.8rem", color: "#64748b" }}>{new Date(o.createdAt).toLocaleDateString()}</span>
                                                </div>
                                                <div style={{ fontSize: "0.85rem", color: "#64748b", marginTop: "0.3rem" }}>Customer ID: {o.customerId}</div>
                                            </div>
                                        ))
                                    )}
                                </div>
                            </div>

                            {/* Completed Box */}
                            <div style={{ background: "white", padding: "1.5rem", borderRadius: "12px", boxShadow: "0 4px 6px -1px rgba(0,0,0,0.1), 0 2px 4px -1px rgba(0,0,0,0.06)", border: "1px solid #f1f5f9" }}>
                                <h3 style={{ marginBottom: "1.5rem", color: "#16a34a", display: "flex", justifyContent: "space-between", alignItems: "center" }}>
                                    Completed
                                    <span style={{ background: "#dcfce7", color: "#166534", padding: "0.2rem 0.8rem", borderRadius: "8px", fontSize: "0.85rem", fontWeight: "bold" }}>{completedOrders.length}</span>
                                </h3>
                                <div style={{ maxHeight: "400px", overflowY: "auto", paddingRight: "0.5rem" }}>
                                    {ordersLoading ? <p>Loading orders...</p> : completedOrders.length === 0 ? <p style={{ color: "#94a3b8", fontStyle: "italic" }}>No completed orders found.</p> : (
                                        completedOrders.map(o => (
                                            <div key={o.orderId} onClick={() => navigate(`/orders/${o.orderId}`)} style={{
                                                padding: "1rem",
                                                borderBottom: "1px solid #f1f5f9",
                                                cursor: "pointer",
                                                borderRadius: "8px",
                                                marginBottom: "0.5rem",
                                                transition: "all 0.2s ease"
                                            }} onMouseOver={e => e.currentTarget.style.background = "#f0fdf4"} onMouseOut={e => e.currentTarget.style.background = "transparent"}>
                                                <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
                                                    <span style={{ fontWeight: "bold", fontSize: "1rem" }}>Order #{o.orderId}</span>
                                                    <span style={{ fontSize: "0.8rem", fontWeight: "bold", color: "#166534" }}>{o.totalPrice.toFixed(2)}€</span>
                                                </div>
                                                <div style={{ fontSize: "0.8rem", color: "#64748b", marginTop: "0.3rem" }}>{new Date(o.createdAt).toLocaleDateString()}</div>
                                            </div>
                                        ))
                                    )}
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
}
