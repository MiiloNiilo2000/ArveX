import type { Product } from "~/types/product";
import { ref } from 'vue';

export const useProductStore = defineStore('product', () => {
    let nextId = 1
    const products = ref<Product[]>([
        {id: nextId++, name: 'Toode 1', description: 'Kirjeldus 1', price: 5 },
        {id: nextId++, name: 'Toode 2', description: 'Kirjeldus 2', price: 2.4 },
        {id: nextId++, name: 'Toode 3', description: 'Kirjeldus 3', price: 500 },
        {id: nextId++, name: 'Toode 4', description: 'Kirjeldus 4', price: 99.99 },
        {id: nextId++, name: 'Toode 5', description: 'Kirjeldus 5', price: 1046.49 },
      ]);

    const addProduct = (product: Product) => {
        product.id = nextId++;
        products.value.push(product);
    };

    const deleteProduct = (id: number) => {
        const index = products.value.findIndex((product) => product.id === id);
        if(index !== -1){
            products.value.splice(index, 1)
        }
    };
    return {products, addProduct, deleteProduct}
})