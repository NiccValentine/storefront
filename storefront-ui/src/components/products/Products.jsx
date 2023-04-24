import React from "react";
import { Route, Routes } from "react-router-dom";
import ProductEdit from "./components/ProductEdit";
import ProductSearch from "./components/ProductSearch";
import ProductView from "./components/ProductView";

const Products = () => {
    return (
        <React.Fragment>
            <Routes>
                <Route
                    path={"/"}
                    element={<ProductSearch />}
                />
                <Route
                    path={"/:productId"}
                    element={<ProductView />}
                />
                <Route
                    path={"/:productId/edit"}
                    element={<ProductEdit />}
                />
            </Routes>
        </React.Fragment>
    )
}

export default Products;