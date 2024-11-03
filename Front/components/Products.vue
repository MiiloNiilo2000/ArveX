<template>
  <div class="container">
    <h1 class="text-3xl font-bold mb-6">Tooted / teenused</h1>

    <UButton class="add-product-btn mb-6" @click="navigateToAddProduct">
      Lisa toode
    </UButton>

    <div v-for="(product, index) in products" :key="index" class="bg-green-100 shadow-md rounded-lg p-3 w-1/3 mb-6">
      <h2 class="text-black text-xl font-semibold">{{ product.name }}</h2>
      <p class="text-gray-600">{{ product.description }}</p>
      <p class="text-gray-600">Hind: {{ product.price }}€</p>

      <div class="flex justify-between mt-4">
        <UButton @click="navigateToEditProduct(product.productId)"><i class="edit-icon">✏️</i></UButton>
        <UButton @click="deleteProduct(product.productId)"><Icon name="mdi-light:delete"/></UButton>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
    import { useRouter } from 'vue-router';
    import axios from 'axios';
    import { onMounted, ref, computed } from 'vue';

    const router = useRouter();
    const products = ref([]);

    onMounted(async () => {
      await fetchProducts();
    });

    console.log("Products after fetch:", products);

    const navigateToAddProduct = () => {
        router.push('/products/add');
    };

    const navigateToEditProduct = (productId: Number) => {
        router.push(`/products/edit/${productId}`);
    };
    
    const fetchProducts = async () => {
        try {
            const response = await axios.get('http://localhost:5176/Products/all');
            products.value = response.data; 
        } catch (error) {
            console.error("Error fetching products:", error);
        }
    };
    
    const deleteProduct = async (id) => {
    try {
        await axios.delete(`http://localhost:5176/Products/${id}`);
        products.value = products.value.filter(product => product.ProductId !== id);
        fetchProducts();
    } 
    catch (error) {
        console.error("Error deleting product:", error);
    }
};
</script>