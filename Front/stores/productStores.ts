import type { Product } from "../types/product";
import { ref } from 'vue';
import axios from "axios";
import { defineStore } from 'pinia';


export const useProductStore = defineStore('product', () => {

    const products = ref<Product[]>([]);
    
   
    
    
    const editProduct = (updatedProduct: Product) => {
        const index = products.value.findIndex((product) => product.id === updatedProduct.id);
        if (index !== -1) {
            products.value[index] = { ...updatedProduct };
        }
    };

    const getProductById = (id: number): Product | undefined => {
        return products.value.find(product => product.id === id);
    };
    
    return { products,  editProduct, getProductById };
});