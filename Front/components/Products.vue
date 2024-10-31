<template>
  <div class="container">
    <h1 class="text-3xl font-bold mb-6">Tooted / teenused</h1>

    <UButton class="add-product-btn mb-6" @click="navigateToAddProduct">
      Lisa toode
    </UButton>

    <UTable :columns="columns" :rows="products">
      <template #delete-data="{ row }">
        <UButton @click="deleteProduct(row.productId)" color="gray" variant="ghost" icon="mdi-delete" />
      </template>
      <template #edit-data="{ row }">
        <UButton @click="navigateToEditProduct(row.productId)" color="gray" variant="ghost" icon="mdi-pencil" />
      </template>
    </UTable>
  </div>
</template>

<script setup lang="ts">
    import { useRouter } from 'vue-router';
    import axios from 'axios';
    import { onMounted, ref, computed } from 'vue';

    const router = useRouter();
    const products = ref([]);

    const columns = ref([
    { key: 'name', label: 'Nimi' },
    { key: 'description', label: 'Kirjeldus' },
    { key: 'price', label: 'Hind' },
    { key: 'delete', label: 'Kustuta' },
    { key: 'edit', label: 'Muuda' },
    ]);

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