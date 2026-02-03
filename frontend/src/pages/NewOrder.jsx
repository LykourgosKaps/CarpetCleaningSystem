import { useState } from "react";
import client from "../api/client";
import { useNavigate } from "react-router-dom";

// Enums matching backend
const ItemTypes = {
    SYNTHETIC: "Synthetic",
    HANDMADE_WOOL: "Handmade Wool",
    MACHINE_MADE_WOOL: "Machine Made Wool"
};

const CleaningTypes = {
    STANDARD_CLEAN_AND_RETURN: "Standard Clean & Return",
    STANDARD_CLEAN_AND_STORAGE: "Standard Clean & Storage",
    DRY_CLEAN_AND_RETURN: "Dry Clean & Return",
    DRY_CLEAN_AND_STORAGE: "Dry Clean & Storage",
    STORAGE_ONLY: "Storage Only"
};

// Pricing Constants (mirrored from backend for estimation)
const PRICING = {
    FLAT: {
        0: 4.00, // STANDARD_CLEAN_AND_RETURN
        1: 4.50, // STANDARD_CLEAN_AND_STORAGE
        2: 6.50, // DRY_CLEAN_AND_RETURN
        3: 7.00, // DRY_CLEAN_AND_STORAGE
        4: 3.50  // STORAGE_ONLY
    },
    RATE: {
        0: 1.20, // SYNTHETIC
        1: 1.50, // HANDMADE_WOOL
        2: 2.00  // MACHINE_MADE_WOOL
    }
};

export default function NewOrder() {
    const [customerId, setCustomerId] = useState("");
    const [searchPhone, setSearchPhone] = useState("");
    const [lookupResult, setLookupResult] = useState(null);
    const [lookupLoading, setLookupLoading] = useState(false);
    const [pickUpDate, setPickUpDate] = useState(new Date().toISOString().split('T')[0]); // Default today
    const [items, setItems] = useState([]);

    // New Item State
    const [newItem, setNewItem] = useState({
        width: "",
        length: "",
        material: "SYNTHETIC",
        cleaningType: "STANDARD_CLEAN_AND_RETURN"
    });

    const [error, setError] = useState("");
    const navigate = useNavigate();

    const handleLookup = async () => {
        const phone = searchPhone.trim();
        if (!phone) return;
        setLookupLoading(true);
        setError("");
        setLookupResult(null);
        try {
            const res = await client.get(`/Customers/lookup?phone=${encodeURIComponent(phone)}`);
            setLookupResult(res.data);
            setCustomerId(res.data.customerId.toString());
        } catch (err) {
            console.error(err);
            setError("Customer not found with this phone number.");
        } finally {
            setLookupLoading(false);
        }
    };

    const calculateItemPrice = (width, length, materialStr, cleaningStr) => {
        const w = parseFloat(width) || 0;
        const l = parseFloat(length) || 0;
        const surface = w * l;

        const materialIdx = Object.keys(ItemTypes).indexOf(materialStr);
        const cleaningIdx = Object.keys(CleaningTypes).indexOf(cleaningStr);

        if (materialIdx === -1 || cleaningIdx === -1) return 0;

        const flat = PRICING.FLAT[cleaningIdx];
        const rate = PRICING.RATE[materialIdx];

        // UPDATED FORMULA: Multiplication
        return flat * rate * surface;
    };

    const handleAddItem = (e) => {
        e.preventDefault();

        const width = parseFloat(newItem.width);
        const length = parseFloat(newItem.length);

        if (!width || width <= 0 || !length || length <= 0) {
            setError("Please enter valid positive dimensions.");
            return;
        }

        const estPrice = calculateItemPrice(newItem.width, newItem.length, newItem.material, newItem.cleaningType);

        const itemToAdd = {
            width,
            length,
            material: Object.keys(ItemTypes).indexOf(newItem.material),
            cleaningType: Object.keys(CleaningTypes).indexOf(newItem.cleaningType),
            displayMaterial: ItemTypes[newItem.material],
            displayCleaning: CleaningTypes[newItem.cleaningType],
            estimatedPrice: estPrice
        };

        setItems([...items, itemToAdd]);
        setNewItem({ ...newItem, width: "", length: "" });
        setError("");
    };

    const handleRemoveItem = (index) => {
        const newItems = [...items];
        newItems.splice(index, 1);
        setItems(newItems);
    };

    const totalPrice = items.reduce((sum, item) => sum + item.estimatedPrice, 0);

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (items.length === 0) {
            setError("You must add at least one item to the order.");
            return;
        }
        if (!customerId) {
            setError("Customer ID is required.");
            return;
        }

        try {
            const payload = {
                customerId: parseInt(customerId),
                pickUpDate: new Date(pickUpDate).toISOString(),
                items: items.map(i => ({
                    width: i.width,
                    length: i.length,
                    material: i.material,
                    cleaningType: i.cleaningType
                }))
            };

            const res = await client.post("/Orders", payload);
            navigate(`/orders/${res.data.orderId}`);
        } catch (err) {
            console.error(err);
            let msg = "Failed to create order.";
            if (err.response?.data?.errors) {
                // ASP.NET Validation errors
                const validationErrors = err.response.data.errors;
                msg = Object.values(validationErrors).flat().join(" ");
            } else if (err.response?.data?.detail) {
                // GlobalExceptionMiddleware
                msg = err.response.data.detail;
            } else if (err.response?.data?.message) {
                msg = err.response.data.message;
            } else if (typeof err.response?.data === 'string' && err.response.data) {
                msg = err.response.data;
            }
            setError(msg);
        }
    };

    return (
        <div className="container" style={{ padding: "4rem 1rem", maxWidth: "800px" }}>
            <h1>Create New Order</h1>
            {error && <div className="error-msg">{error}</div>}

            <div className="auth-form-container" style={{ margin: "2rem 0", padding: "2rem" }}>

                <div style={{ background: "#f8fafc", padding: "1.5rem", borderRadius: "8px", border: "1px solid #e2e8f0", marginBottom: "2rem" }}>
                    <h4 style={{ margin: "0 0 1rem 0" }}>Quick Customer Lookup</h4>
                    <div style={{ display: "flex", gap: "1rem" }}>
                        <div className="form-group" style={{ flex: 1, marginBottom: 0 }}>
                            <input
                                type="text"
                                value={searchPhone}
                                onChange={(e) => setSearchPhone(e.target.value)}
                                placeholder="Enter Phone Number (e.g. 698...)"
                                style={{ marginBottom: 0 }}
                            />
                        </div>
                        <button
                            type="button"
                            onClick={handleLookup}
                            className="btn btn-secondary"
                            disabled={lookupLoading}
                        >
                            {lookupLoading ? "Searching..." : "Search"}
                        </button>
                    </div>
                    {lookupResult && (
                        <div style={{ marginTop: "1rem", padding: "1rem", background: "#ecfdf5", border: "1px solid #10b981", borderRadius: "4px", color: "#065f46" }}>
                            <strong>Found:</strong> {lookupResult.firstName} {lookupResult.lastName} (ID: {lookupResult.customerId})<br />
                            <small>Address: {lookupResult.address}</small>
                            {lookupResult.latestOrderId && (
                                <div style={{ marginTop: "0.5rem", fontSize: "0.85rem", color: "#047857" }}>
                                    Latest Order: #{lookupResult.latestOrderId} (Status: {lookupResult.latestOrderStatus})
                                </div>
                            )}
                        </div>
                    )}
                </div>

                <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "2rem", marginBottom: "2rem" }}>
                    <div className="form-group">
                        <label>Customer ID</label>
                        <input
                            type="number"
                            value={customerId}
                            onChange={(e) => setCustomerId(e.target.value)}
                            required
                            placeholder="Enter Customer ID"
                        />
                        <small style={{ color: '#666' }}>ID from search or "Customers" page</small>
                    </div>

                    <div className="form-group">
                        <label>Pick Up Date</label>
                        <input
                            type="date"
                            value={pickUpDate}
                            onChange={(e) => setPickUpDate(e.target.value)}
                            required
                        />
                    </div>
                </div>

                <hr style={{ margin: "2rem 0", border: 0, borderTop: "1px solid #eee" }} />

                <h3 style={{ marginBottom: "1rem" }}>Items ({items.length})</h3>

                {items.length > 0 && (
                    <>
                        <table style={{ width: "100%", marginBottom: "1rem", borderCollapse: "collapse" }}>
                            <thead>
                                <tr style={{ background: "#f1f5f9", textAlign: "left" }}>
                                    <th style={{ padding: "0.5rem" }}>Material</th>
                                    <th style={{ padding: "0.5rem" }}>Cleaning</th>
                                    <th style={{ padding: "0.5rem" }}>Dims (m)</th>
                                    <th style={{ padding: "0.5rem" }}>Est. Price</th>
                                    <th style={{ padding: "0.5rem" }}>Action</th>
                                </tr>
                            </thead>
                            <tbody>
                                {items.map((item, idx) => (
                                    <tr key={idx} style={{ borderBottom: "1px solid #eee" }}>
                                        <td style={{ padding: "0.5rem" }}>{item.displayMaterial}</td>
                                        <td style={{ padding: "0.5rem" }}>{item.displayCleaning}</td>
                                        <td style={{ padding: "0.5rem" }}>{item.width} x {item.length}</td>
                                        <td style={{ padding: "0.5rem" }}>{item.estimatedPrice.toFixed(2)} €</td>
                                        <td style={{ padding: "0.5rem" }}>
                                            <button
                                                type="button"
                                                onClick={() => handleRemoveItem(idx)}
                                                style={{ color: "red", background: "none", border: "none", cursor: "pointer" }}
                                            >
                                                Remove
                                            </button>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                        <div style={{ textAlign: "right", fontWeight: "bold", fontSize: "1.2rem", marginBottom: "2rem" }}>
                            Total Estimated: {totalPrice.toFixed(2)} €
                        </div>
                    </>
                )}

                <div style={{ background: "#f8fafc", padding: "1.5rem", borderRadius: "8px", border: "1px solid #e2e8f0" }}>
                    <h4 style={{ margin: "0 0 1rem 0" }}>Add Item</h4>
                    <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "1rem" }}>

                        <div className="form-group">
                            <label>Material</label>
                            <select
                                value={newItem.material}
                                onChange={(e) => setNewItem({ ...newItem, material: e.target.value })}
                                style={{ width: "100%", padding: "0.5rem" }}
                            >
                                {Object.keys(ItemTypes).map(key => (
                                    <option key={key} value={key}>{ItemTypes[key]}</option>
                                ))}
                            </select>
                        </div>

                        <div className="form-group">
                            <label>Cleaning Type</label>
                            <select
                                value={newItem.cleaningType}
                                onChange={(e) => setNewItem({ ...newItem, cleaningType: e.target.value })}
                                style={{ width: "100%", padding: "0.5rem" }}
                            >
                                {Object.keys(CleaningTypes).map(key => (
                                    <option key={key} value={key}>{CleaningTypes[key]}</option>
                                ))}
                            </select>
                        </div>

                        <div className="form-group">
                            <label>Width (m)</label>
                            <input
                                type="number"
                                step="0.01"
                                value={newItem.width}
                                onChange={(e) => setNewItem({ ...newItem, width: e.target.value })}
                                placeholder="e.g. 2.5"
                            />
                        </div>

                        <div className="form-group">
                            <label>Length (m)</label>
                            <input
                                type="number"
                                step="0.01"
                                value={newItem.length}
                                onChange={(e) => setNewItem({ ...newItem, length: e.target.value })}
                                placeholder="e.g. 3.0"
                            />
                        </div>
                    </div>
                    <button
                        type="button"
                        onClick={handleAddItem}
                        className="btn btn-outline"
                        style={{ marginTop: "1rem", width: "100%" }}
                    >
                        + Add Item to Order
                    </button>

                    {/* Live Preview */}
                    {newItem.width && newItem.length && (
                        <div style={{ marginTop: '0.5rem', padding: '0.5rem', background: '#f0f9ff', borderRadius: '4px', border: '1px solid #bae6fd', color: '#0369a1', fontSize: '0.9rem' }}>
                            Preview: {calculateItemPrice(newItem.width, newItem.length, newItem.material, newItem.cleaningType).toFixed(2)} €
                            <span style={{ marginLeft: '10px', fontSize: '0.7rem', color: '#666' }}>
                                ( {PRICING.FLAT[Object.keys(CleaningTypes).indexOf(newItem.cleaningType)]} * {PRICING.RATE[Object.keys(ItemTypes).indexOf(newItem.material)]} * {(newItem.width * newItem.length).toFixed(1)} m² )
                            </span>
                        </div>
                    )}
                </div>

                <div style={{ marginTop: "2rem", textAlign: "right" }}>
                    <button
                        onClick={handleSubmit}
                        className="btn btn-primary"
                        style={{ fontSize: "1.2rem", padding: "0.8rem 2rem" }}
                        disabled={items.length === 0}
                    >
                        Create Order
                    </button>
                </div>
            </div>
        </div>
    );
}
