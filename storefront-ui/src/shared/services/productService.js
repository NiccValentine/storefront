import StoreFrontAxiosService from "./axios/StoreFrontAxiosService";

export default class ProductService {
    async search(searchTerm) {
        return await StoreFrontAxiosService.get('/products/search', {
            params: {
                productName: searchTerm
            }
        })
    }

    get() {
        return StoreFrontAxiosService.get("/products")
    }

    getSingle(productId) {
        return StoreFrontAxiosService.get(`/products/${productId}`)
    }

    post(product) {
        return StoreFrontAxiosService.post("/products", product)
    }

    put(product, productId) {
        return StoreFrontAxiosService.put(`/products/${productId}`, product)
    }

    delete(productId) {
        return StoreFrontAxiosService.delete(`/products/${productId}`)
    }

    getProductsByStoreId(storeId) {
        return StoreFrontAxiosService.get(`/stores/${storeId}/products`)
    }

    getProductsNotMatchingStoreId(storeId) {
        return StoreFrontAxiosService.get(`/stores/${storeId}/products/not`)
    }
}