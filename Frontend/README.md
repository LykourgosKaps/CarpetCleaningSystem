# Carpet Cleaning System - Frontend

This is the frontend application for the Carpet Cleaning System, built with **React** and **Vite**, using a **Premium & Humanized** design aesthetic.

## Features
- **Authentication**: Secure Login and Registration (JWT based).
- **Dashboard**: Task-based dashboard for viewing and managing orders.
- **Order Management**: Create new orders, search for existing orders, and view details.
- **Admin**: Customer lookup functionality.
- **Responsive Design**: Optimized for desktop and mobile.

## Prerequisites
- Node.js (v18 or higher recommended)
- The Backend API (`CarpetCleaningSystem.API`) must be running on `https://localhost:7001` (or configure proxy in `vite.config.js`).

## Getting Started

1.  **Install Dependencies**
    ```bash
    npm install
    ```

2.  **Run Development Server**
    ```bash
    npm run dev
    ```
    Access the app at `http://localhost:5173`.

## Build and Deploy

To create a production build:

1.  **Build**
    ```bash
    npm run build
    ```
    This will generate the static files in the `dist/` folder.

2.  **Deploy**
    - You can deploy the contents of the `dist/` folder to any static host (Netlify, Vercel, IIS, etc.).
    - Ensure your web server handles the specialized routing (SPA Fallback) by redirecting all 404s to `index.html`.

## Project Structure
- `src/api`: Axios client setup.
- `src/components`: Shared UI components (Navbar, Layout, ProtectedRoute).
- `src/context`: AuthContext for global state.
- `src/pages`: Application views (Home, Login, Dashboard, etc.).
- `src/index.css`: Global "Premium" styles.

## Authors
- Coding Factory 8 Student
