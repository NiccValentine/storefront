import StoreFrontAxiosService from "./axios/StoreFrontAxiosService";

export default class StoreService {
    async search(searchTerm) {
        return await StoreFrontAxiosService.get('/stores/search', {
            params: {
                storeName: searchTerm
            }
        })
    }

    get() {
        return StoreFrontAxiosService.get("/stores")
    }

    getStoresByProductId(productId) {
        return StoreFrontAxiosService.get(`/product/${productId}/stores`)
    }

    getSingle(storeId) {
        return StoreFrontAxiosService.get(`/stores/${storeId}`)
    }

    post(store) {
        return StoreFrontAxiosService.post("/stores", store)
    }

    put(store, storeId) {
        return StoreFrontAxiosService.put(`/stores/${storeId}`, store)
    }

    delete(storeId) {
        return StoreFrontAxiosService.delete(`/stores/${storeId}`)
    }
}