<template>
    <div class="container">
      <h1 class="text-3xl font-bold mb-6">Tooted / teenused</h1>
  
      <UButton class="add-product-btn mb-6" @click="navigateToAddProduct">
        Lisa toode
      </UButton>
  
      <UTable :columns="columns" :rows="products">
        <template #delete-data="{ row }">
          <UButton @click="deleteProduct(row.id)" color="gray" variant="ghost" icon="mdi-delete" />
        </template>
        <template #edit-data="{ row }">
          <UButton @click="navigateToEditProduct(row.id)" color="gray" variant="ghost" icon="mdi-pencil" />
        </template>
      </UTable>
    </div>
  </template>
  
  <script setup lang="ts">
      import { useProductStore } from '../stores/productStores';
      import { useRouter } from 'vue-router';
      import axios from 'axios';
      import { onMounted, ref } from 'vue';
  
      const router = useRouter();
      const {products, fetchProducts, deleteProduct } = useProductStore();
  
    
  
      const columns = ref([
      { key: 'name', label: 'Nimi' },
      { key: 'description', label: 'Kirjeldus' },
      { key: 'price', label: 'Hind' },
      { key: 'delete', label: 'Kustuta' },
      { key: 'edit', label: 'Muuda' },
      ]);
  
      const navigateToAddProduct = () => {
          router.push('/products/add');
      };
  
      const navigateToEditProduct = (productId: Number) => {
          router.push(`/products/edit/${productId}`);
      };
      
      onMounted(() => {
        fetchProducts();
      });
  
  </script>