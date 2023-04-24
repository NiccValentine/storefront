import React from "react";
import { Route, Routes } from "react-router-dom";
import StoreEdit from "./componants/StoreEdit";
import StoreSearch from "./componants/StoreSearch";
import StoreView from "./componants/StoreView";

const Stores = () => {
    return (
        <React.Fragment>
            <Routes>
                <Route
                    path={"/"}
                    element={<StoreSearch />}
                />
                <Route
                    path={"/:storeId"}
                    element={<StoreView />}
                />
                <Route
                    path={"/:storeId/edit"}
                    element={<StoreEdit />}
                />
            </Routes>
        </React.Fragment>
    )
}

export default Stores;