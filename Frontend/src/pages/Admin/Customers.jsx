import { useState } from "react";
import client from "../../api/client";

export default function CustomerList() {
    const [searchId, setSearchId] = useState("");
    const [customer, setCustomer] = useState(null);
    const [error, setError] = useState("");

    // Create Customer State
    const [showCreateForm, setShowCreateForm] = useState(false);
    const [newCustomer, setNewCustomer] = useState({
        firstName: "",
        lastName: "",
        phoneNumber: "",
        address: ""
    });
    const [createError, setCreateError] = useState("");
    const [createdCustomerId, setCreatedCustomerId] = useState(null);

    const handleSearch = async (e) => {
        e.preventDefault();
        setError("");
        setCustomer(null);
        try {
            const res = await client.get(`/Customers/${searchId}`);
            setCustomer(res.data);
        } catch (err) {
            setError("Customer not found");
        }
    };

    const handleCreateSubmit = async (e) => {
        e.preventDefault();
        setCreateError("");
        setCreatedCustomerId(null);

        // Basic frontend validation if needed, though backend handles it too
        if (!newCustomer.firstName || !newCustomer.lastName || !newCustomer.phoneNumber || !newCustomer.address) {
            setCreateError("All fields are required.");
            return;
        }

        try {
            const res = await client.post("/Customers", newCustomer);
            if (res.status === 201) {
                // The backend returns the created object differently depending on implementation, 
                // typically the location header or the body. 
                // Based on controller: CreatedAtAction(nameof(GetCustomerById), new { customerId = response.CustomerId }, response);
                // The body should be the response object containing CustomerId.
                setCreatedCustomerId(res.data.customerId);

                // Reset form
                setNewCustomer({ firstName: "", lastName: "", phoneNumber: "", address: "" });
                setShowCreateForm(false);
            }
        } catch (err) {
            console.error(err);
            setCreateError("Failed to create customer. Ensure valid data (Phone max 15, Address min 4 chars).");
        }
    };

    return (
        <div className="container" style={{ padding: "4rem 1rem" }}>
            <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: '2rem' }}>
                <h1>Customer Management</h1>
                <button
                    className="btn btn-primary"
                    onClick={() => setShowCreateForm(!showCreateForm)}
                >
                    {showCreateForm ? "Cancel Creation" : "Create New Customer"}
                </button>
            </div>

            {/* Success Message for Creation */}
            {createdCustomerId && (
                <div style={{
                    padding: "1rem",
                    background: "#dcfce7",
                    color: "#166534",
                    border: "1px solid #bbf7d0",
                    borderRadius: "8px",
                    marginBottom: "2rem"
                }}>
                    <strong>Success!</strong> Customer Created. ID: <strong>{createdCustomerId}</strong>
                    <button
                        onClick={() => {
                            setSearchId(createdCustomerId);
                            setCreatedCustomerId(null);
                        }}
                        style={{ marginLeft: "1rem", textDecoration: "underline", background: "none", border: "none", cursor: "pointer", color: "inherit" }}
                    >
                        Search this ID
                    </button>
                </div>
            )}

            {/* Create Customer Form */}
            {showCreateForm && (
                <div style={{ background: "#f8fafc", padding: "2rem", borderRadius: "12px", marginBottom: "3rem", border: "1px solid #e2e8f0" }}>
                    <h2 style={{ fontSize: "1.5rem", marginBottom: "1rem" }}>New Customer</h2>
                    {createError && <div className="error-msg" style={{ marginBottom: "1rem" }}>{createError}</div>}

                    <form onSubmit={handleCreateSubmit}>
                        <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "1rem", marginBottom: "1rem" }}>
                            <div className="form-group">
                                <label>First Name</label>
                                <input
                                    type="text"
                                    value={newCustomer.firstName}
                                    onChange={(e) => setNewCustomer({ ...newCustomer, firstName: e.target.value })}
                                    required
                                    minLength={2}
                                    maxLength={50}
                                />
                            </div>
                            <div className="form-group">
                                <label>Last Name</label>
                                <input
                                    type="text"
                                    value={newCustomer.lastName}
                                    onChange={(e) => setNewCustomer({ ...newCustomer, lastName: e.target.value })}
                                    required
                                    minLength={2}
                                    maxLength={50}
                                />
                            </div>
                        </div>

                        <div className="form-group" style={{ marginBottom: "1rem" }}>
                            <label>Phone Number</label>
                            <input
                                type="tel"
                                value={newCustomer.phoneNumber}
                                onChange={(e) => setNewCustomer({ ...newCustomer, phoneNumber: e.target.value })}
                                required
                                maxLength={15}
                            />
                        </div>

                        <div className="form-group" style={{ marginBottom: "1.5rem" }}>
                            <label>Address</label>
                            <input
                                type="text"
                                value={newCustomer.address}
                                onChange={(e) => setNewCustomer({ ...newCustomer, address: e.target.value })}
                                required
                                minLength={4}
                                maxLength={50}
                            />
                        </div>

                        <button type="submit" className="btn btn-primary">Save Customer</button>
                    </form>
                </div>
            )}

            <p>Search for a customer by ID to view details.</p>

            <form onSubmit={handleSearch} style={{ marginTop: "1rem", maxWidth: "400px" }}>
                <div style={{ display: "flex", gap: "0.5rem" }}>
                    <input
                        type="number"
                        placeholder="Customer ID"
                        value={searchId}
                        onChange={(e) => setSearchId(e.target.value)}
                        required
                    />
                    <button type="submit" className="btn btn-primary">Search</button>
                </div>
            </form>

            {error && <div className="error-msg">{error}</div>}

            {customer && (
                <div style={{ marginTop: "2rem", background: "white", padding: "2rem", borderRadius: "12px", boxShadow: "var(--shadow-md)" }}>
                    <h3>Customer Details</h3>
                    <p><strong>ID:</strong> {customer.customerId || customer.id}</p>
                    <p><strong>Name:</strong> {customer.firstName} {customer.lastName}</p>
                    <p><strong>Phone:</strong> {customer.phoneNumber || "N/A"}</p>
                    <p><strong>Address:</strong> {customer.address || "N/A"}</p>
                </div>
            )}
        </div>
    );
}
