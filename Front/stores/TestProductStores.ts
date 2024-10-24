import type { Product } from "../types/product";
import { ref } from 'vue';
import axios from "axios";

export const useProductStore = defineStore('product', () => {

    const products = ref<Product[]>([]);

    const fetchProducts = async () => {
        try {
            const response = await axios.get('http://localhost:5176/Products/all');
            products.value = response.data; 
        } catch (error) {
            console.error("Error fetching products:", error);
        }
    };

    const addProduct = async (product) => {
        try {
            await axios.post('http://localhost:5176/Products', product);
            await fetchProducts();
        } catch (error) {
            console.error("Error adding product:", error);
        }
    };

    const deleteProduct = async (id: number) => {
        try {
            await axios.delete(`http://localhost:5176/Products/${id}`);
            const index = products.value.findIndex((product) => product.id === id);
            if (index !== -1) {
                products.value.splice(index, 1);
            }
        } catch (error) {
            console.error("Error deleting product:", error);
        }
    };
    
    
    const editProduct = (updatedProduct: Product) => {
        const index = products.value.findIndex((product) => product.id === updatedProduct.id);
        if (index !== -1) {
            products.value[index] = { ...updatedProduct };
        }
    };

    const getProductById = (id: number): Product | undefined => {
        return products.value.find(product => product.id === id);
    };
    
    return { products, fetchProducts, addProduct, deleteProduct, editProduct, getProductById };
})