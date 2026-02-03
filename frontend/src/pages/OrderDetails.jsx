import { useState, useEffect, useContext } from "react";
import { useParams, useNavigate } from "react-router-dom";
import client from "../api/client";
import AuthContext from "../context/AuthContext";

const ItemTypes = {
    0: "Synthetic",
    1: "Handmade Wool",
    2: "Machine Made Wool",
    "SYNTHETIC": "Synthetic",
    "HANDMADE_WOOL": "Handmade Wool",
    "MACHINE_MADE_WOOL": "Machine Made Wool"
};

const CleaningTypes = {
    0: "Standard Clean & Return",
    1: "Standard Clean & Storage",
    2: "Dry Clean & Return",
    3: "Dry Clean & Storage",
    4: "Storage Only",
    "STANDARD_CLEAN_AND_RETURN": "Standard Clean & Return",
    "STANDARD_CLEAN_AND_STORAGE": "Standard Clean & Storage",
    "DRY_CLEAN_AND_RETURN": "Dry Clean & Return",
    "DRY_CLEAN_AND_STORAGE": "Dry Clean & Storage",
    "STORAGE_ONLY": "Storage Only"
};

const PRICING = {
    FLAT: { 0: 4, 1: 4.5, 2: 6.5, 3: 7, 4: 3.5 },
    RATE: { 0: 1.2, 1: 1.5, 2: 2 }
};

export default function OrderDetails() {
    const { id } = useParams();
    const { user } = useContext(AuthContext);
    const [order, setOrder] = useState(null);
    const [loading, setLoading] = useState(true);
    const [actionLoading, setActionLoading] = useState(false);
    const [error, setError] = useState("");
    const navigate = useNavigate();

    // Editing State
    const [editingItem, setEditingItem] = useState(null); // { itemNo, width, length, material, cleaningType }
    const [showAddItem, setShowAddItem] = useState(false);
    const [newItem, setNewItem] = useState({ width: "", length: "", material: 0, cleaningType: 0 });

    const fetchOrder = async () => {
        setLoading(true);
        try {
            const res = await client.get(`/Orders/${id}`);
            setOrder(res.data);
            setError("");
        } catch (err) {
            setError("Order not found or access denied.");
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchOrder();
    }, [id]);

    const handleAction = async (endpoint) => {
        setActionLoading(true);
        setError("");
        try {
            await client.post(`/Orders/${id}/${endpoint}`);
            await fetchOrder();
        } catch (err) {
            setError(`Action failed: ${err.response?.data || err.message}`);
        } finally {
            setActionLoading(false);
        }
    };

    const handleUpdateItem = async (itemNo, field, value) => {
        if (field === "dimensions") {
            if (!value.width || !value.length) {
                alert("Dimensions cannot be empty.");
                return;
            }
        } else if (!value && value !== 0) {
            alert("This field cannot be empty.");
            return;
        }

        setActionLoading(true);
        try {
            let endpoint = "";
            let payload = {};
            if (field === "material") {
                endpoint = "material";
                payload = { material: parseInt(value) };
            } else if (field === "dimensions") {
                endpoint = "dimensions";
                payload = { width: parseFloat(value.width), length: parseFloat(value.length) };
            } else if (field === "cleaningType") {
                endpoint = "cleaning-type";
                payload = { cleaningType: parseInt(value) };
            }
            await client.put(`/Orders/${id}/items/${itemNo}/${endpoint}`, payload);
            await fetchOrder();
            setEditingItem(null);
        } catch (err) {
            setError("Update failed.");
        } finally {
            setActionLoading(false);
        }
    };

    const handleAddItem = async () => {
        if (!newItem.width || !newItem.length) {
            alert("Please provide both width and length.");
            return;
        }

        setActionLoading(true);
        try {
            await client.post(`/Orders/${id}/items`, {
                width: parseFloat(newItem.width),
                length: parseFloat(newItem.length),
                material: parseInt(newItem.material),
                cleaningType: parseInt(newItem.cleaningType)
            });
            await fetchOrder();
            setShowAddItem(false);
            setNewItem({ width: "", length: "", material: 0, cleaningType: 0 });
        } catch (err) {
            setError("Failed to add item.");
        } finally {
            setActionLoading(false);
        }
    };

    if (loading) return <div className="container" style={{ padding: "4rem" }}>Loading...</div>;
    if (error && !order) return <div className="container" style={{ padding: "4rem", color: "red" }}>{error}</div>;
    if (!order) return null;

    const statusStr = String(order.status).toUpperCase();
    const isDraftOrSubmitted = statusStr === "0" || statusStr === "DRAFT" || statusStr === "1" || statusStr === "SUBMITTED";
    const isAdmin = user?.roles?.includes("Admin");

    // RBAC
    const canSubmit = (statusStr === "0" || statusStr === "DRAFT") && order.items?.length > 0;
    const canProcess = statusStr === "1" || statusStr === "SUBMITTED";
    const canComplete = isAdmin && (statusStr === "2" || statusStr === "IN_PROGRESS");
    const canCancel = isAdmin && (statusStr === "0" || statusStr === "DRAFT" || statusStr === "1" || statusStr === "SUBMITTED");
    const canEdit = isDraftOrSubmitted;

    const calculateEst = (item) => {
        const mat = typeof item.itemType === 'string' ? Object.keys(PRICING.RATE).find(k => ItemTypes[k] === item.itemType || k === item.itemType) : item.itemType;
        const cln = typeof item.cleaningType === 'string' ? Object.keys(PRICING.FLAT).find(k => CleaningTypes[k] === item.cleaningType || k === item.cleaningType) : item.cleaningType;
        const rate = PRICING.RATE[mat] || 0;
        const flat = PRICING.FLAT[cln] || 0;
        return (rate * flat * item.surface).toFixed(2);
    };

    return (
        <div className="container" style={{ padding: "4rem 1rem" }}>
            <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: "2rem" }}>
                <button onClick={() => navigate("/dashboard")} className="btn btn-outline" >&larr; Dashboard</button>
                <div style={{ display: 'flex', gap: '0.5rem' }}>
                    {canSubmit && <button className="btn btn-primary" onClick={() => handleAction("submit")} disabled={actionLoading}>Submit</button>}
                    {canProcess && <button className="btn btn-primary" onClick={() => handleAction("start-processing")} disabled={actionLoading}>Process</button>}
                    {canComplete && <button className="btn btn-primary" style={{ background: "#16a34a" }} onClick={() => handleAction("complete")} disabled={actionLoading}>Complete</button>}
                    {canCancel && <button className="btn btn-outline" style={{ color: "red" }} onClick={() => handleAction("cancel")} disabled={actionLoading}>Cancel</button>}
                </div>
            </div>

            <div style={{ background: "white", padding: "2rem", borderRadius: "12px", boxShadow: "var(--shadow-md)" }}>
                <header style={{ display: 'flex', justifyContent: 'space-between', marginBottom: "2rem" }}>
                    <h1>Order #{order.orderId} <small style={{ fontSize: '0.5em', color: '#666' }}>({isAdmin ? "Admin View" : "Employee View"})</small></h1>
                    <span style={{ background: "#f1f5f9", padding: "0.5rem 1rem", borderRadius: "99px" }}>{statusStr}</span>
                </header>

                <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "2rem", marginBottom: "3rem" }}>
                    <div>
                        <h3 style={{ color: "#64748b", textTransform: "uppercase", fontSize: "0.8rem" }}>Customer</h3>
                        <p>ID: {order.customerId}</p>
                    </div>
                    <div>
                        <h3 style={{ color: "#64748b", textTransform: "uppercase", fontSize: "0.8rem" }}>Summary</h3>
                        <p>Items: {order.items?.length || 0}</p>
                        <p>Total: {order.totalPrice > 0 ? `${order.totalPrice.toFixed(2)} €` : "Estimated"}</p>
                    </div>
                </div>

                <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                    <h3>Items</h3>
                    {canEdit && <button className="btn btn-outline" onClick={() => setShowAddItem(!showAddItem)}>+ Add Item</button>}
                </div>

                {showAddItem && (
                    <div style={{ background: "#f8fafc", padding: "1rem", margin: "1rem 0", borderRadius: "8px", border: "1px dashed #cbd5e1" }}>
                        <div style={{ display: 'grid', gridTemplateColumns: 'repeat(4, 1fr)', gap: '1rem' }}>
                            <input type="number" placeholder="W" value={newItem.width} onChange={e => setNewItem({ ...newItem, width: e.target.value })} />
                            <input type="number" placeholder="L" value={newItem.length} onChange={e => setNewItem({ ...newItem, length: e.target.value })} />
                            <select value={newItem.material} onChange={e => setNewItem({ ...newItem, material: e.target.value })}>
                                {Object.entries(ItemTypes).filter(([k]) => !isNaN(k)).map(([k, v]) => <option key={k} value={k}>{v}</option>)}
                            </select>
                            <select value={newItem.cleaningType} onChange={e => setNewItem({ ...newItem, cleaningType: e.target.value })}>
                                {Object.entries(CleaningTypes).filter(([k]) => !isNaN(k)).map(([k, v]) => <option key={k} value={k}>{v}</option>)}
                            </select>
                        </div>
                        <button className="btn btn-primary" style={{ marginTop: '1rem', width: '100%' }} onClick={handleAddItem}>Save Item</button>
                    </div>
                )}

                <table style={{ width: "100%", borderCollapse: "collapse", marginTop: "1rem" }}>
                    <thead>
                        <tr style={{ textAlign: "left", color: "#64748b", borderBottom: "1px solid #eee" }}>
                            <th style={{ padding: "1rem" }}>#</th>
                            <th style={{ padding: "1rem" }}>Material</th>
                            <th style={{ padding: "1rem" }}>Service</th>
                            <th style={{ padding: "1rem" }}>Size</th>
                            <th style={{ padding: "1rem" }}>Price</th>
                            {canEdit && <th style={{ padding: "1rem" }}>Action</th>}
                        </tr>
                    </thead>
                    <tbody>
                        {order.items?.map((item) => (
                            <tr key={item.itemNo} style={{ borderBottom: "1px solid #f8fafc" }}>
                                <td style={{ padding: "1rem" }}>{item.itemNo}</td>
                                <td style={{ padding: "1rem" }}>
                                    {editingItem?.itemNo === item.itemNo ? (
                                        <select defaultValue={item.itemType} onChange={(e) => handleUpdateItem(item.itemNo, 'material', e.target.value)}>
                                            {Object.entries(ItemTypes).filter(([k]) => !isNaN(k)).map(([k, v]) => <option key={k} value={k}>{v}</option>)}
                                        </select>
                                    ) : (ItemTypes[item.itemType] || item.itemType)}
                                </td>
                                <td style={{ padding: "1rem" }}>
                                    {editingItem?.itemNo === item.itemNo ? (
                                        <select defaultValue={item.cleaningType} onChange={(e) => handleUpdateItem(item.itemNo, 'cleaningType', e.target.value)}>
                                            {Object.entries(CleaningTypes).filter(([k]) => !isNaN(k)).map(([k, v]) => <option key={k} value={k}>{v}</option>)}
                                        </select>
                                    ) : (CleaningTypes[item.cleaningType] || item.cleaningType)}
                                </td>
                                <td style={{ padding: "1rem" }}>
                                    {editingItem?.itemNo === item.itemNo ? (
                                        <div style={{ display: 'flex', gap: '5px' }}>
                                            <input style={{ width: '40px' }} type="number" defaultValue={item.width} onBlur={(e) => handleUpdateItem(item.itemNo, 'dimensions', { width: e.target.value, length: item.length })} /> x
                                            <input style={{ width: '40px' }} type="number" defaultValue={item.length} onBlur={(e) => handleUpdateItem(item.itemNo, 'dimensions', { width: item.width, length: e.target.value })} />
                                        </div>
                                    ) : `${item.width} x ${item.length} m`}
                                </td>
                                <td style={{ padding: "1rem" }}>
                                    {item.itemPrice > 0 ? `${item.itemPrice.toFixed(2)} €` : `${calculateEst(item)} € (Est.)`}
                                </td>
                                {canEdit && (
                                    <td style={{ padding: "1rem" }}>
                                        <button className="btn btn-outline" style={{ padding: '0.2rem 0.5rem' }} onClick={() => setEditingItem(editingItem?.itemNo === item.itemNo ? null : item)}>
                                            {editingItem?.itemNo === item.itemNo ? "Done" : "Edit"}
                                        </button>
                                    </td>
                                )}
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
}
