import type { Product } from "../types/product";
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { defineStore } from 'pinia';



export const useProductStore = defineStore('product', () => {

    const router = useRouter();
    const state = reactive({
        products: ref<Product[]>([]),
        companies: ref<Company[]>([]),
        selectedCompanyId: 0
      });
       
    const editProduct = (updatedProduct: Product) => {
        const index = state.products.findIndex((product) => product.productId === updatedProduct.productId);
        if (index !== -1) {
            state.products[index] = { ...updatedProduct };
        }
    };
      
    const navigateToEditProduct = (productId: number) => {
    router.push(`/products/edit/${productId}`);
    };
    const navigateToAddProduct = () => {
        router.push(`/products`);
    };
    const getCompanyNameById = (companyId: number) => {
        const company = state.companies.find(c => c.companyId === companyId);
        return company ? company.name : 'Ettevõtet ei leitud';
    };
    const getProductById = (id: number): Product | undefined => {
        return state.products.find(product => product.productId === id);
    };
    
    return { editProduct,
             navigateToAddProduct,
             navigateToEditProduct, 
             getProductById, 
             getCompanyNameById,
             state
            };
});
