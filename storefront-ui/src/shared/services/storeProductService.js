import StoreFrontAxiosService from "./axios/StoreFrontAxiosService";

export default class StoreProductService {
    post(storeProduct) {
        return StoreFrontAxiosService.post("/storeproducts", storeProduct)
    }

    delete(storeId, productId) {
        return StoreFrontAxiosService.delete(`/storeproducts/${storeId}/products/${productId}`)
    }
}