import type { Product } from "../types/product";
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { defineStore } from 'pinia';



export const useProductStore = defineStore('product', () => {

    const products = ref<Product[]>([]);
    const router = useRouter();
       
    const editProduct = (updatedProduct: Product) => {
        const index = products.value.findIndex((product) => product.productId === updatedProduct.productId);
        if (index !== -1) {
            products.value[index] = { ...updatedProduct };
        }
    };

    const navigateToAddProduct = () => {
        router.push('/products/add');
      };
      
    const navigateToEditProduct = (productId: number) => {
    router.push(`/products/edit/${productId}`);
    };
    

    const getProductById = (id: number): Product | undefined => {
        return products.value.find(product => product.productId === id);
    };
    
    return { products,  editProduct, navigateToAddProduct, navigateToEditProduct, getProductById };
});
