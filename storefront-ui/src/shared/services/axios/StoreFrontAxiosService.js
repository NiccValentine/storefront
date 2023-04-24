import axios from "axios";

export default axios.create({
    baseURL: process.env.REACT_APP_STOREFRONT_API,
    responseType: "json"
});