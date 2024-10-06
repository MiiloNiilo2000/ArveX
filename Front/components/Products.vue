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
          <UButton @click="editProduct"><i class="edit-icon">✏️</i></UButton>
          <UButton @click="deleteProduct(product.id)"><Icon name="mdi-light:delete"/></UButton>
        </div>
      </div>
    </div>
</template>

<script setup lang="ts">
    import { useProductStore } from '~/stores/productStores';
    import { useRouter } from 'vue-router';

    const router = useRouter();
    const {products, deleteProduct, addProduct} = useProductStore();

    const editProduct = () => {
        alert('Product editing not implemented yet!')
    }

    const navigateToAddProduct = () => {
        router.push('/add-product');
    }
</script>