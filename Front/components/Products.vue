<template>
  <div class="container">
    <h1 class="text-3xl font-bold mb-6">{{ getCompanyNameById(selectedCompanyId) }}</h1>
    <h1 class="text-2xl font-bold mb-6">Tooted / teenused</h1>

    <div class="mb-6">
      <label for="companySelect" class="block text-sm font-medium">Vali ettevõte:</label>
      <select v-model="selectedCompanyId" id="companySelect" @change="onCompanyChange" class="mt-1 block w-full border-gray-300 rounded-md shadow-sm" style="width: 200px">
        <option v-for="company in companies" :key="company.companyId" :value="company.companyId">
          {{ company.name }}
        </option>
      </select>
    </div>

    <UButton class="add-product-btn mb-6" @click="navigateToAddProduct">
      Lisa toode
    </UButton>

    <div v-for="(product, index) in products" :key="index" class="bg-green-100 shadow-md rounded-lg p-3 w-1/3 mb-6">
      <h2 class="text-black text-xl font-semibold">{{ product.name }}</h2>
      <p class="text-gray-600">{{ product.description }}</p>
      <p class="text-gray-600">Hind: {{ product.price }}€</p>
      <p class="text-gray-600">{{ getCompanyNameById(product.companyId) }}</p>

      <div class="flex justify-between mt-4">
        <UButton @click="navigateToEditProduct(product.productId)" title="Muuda"><i class="edit-icon">✏️</i></UButton>
        <UButton @click="deleteProduct(product.productId)" title="Kustuta"><Icon name="mdi-light:delete"/></UButton>
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
    const companies = ref([]);
    const selectedCompanyId = ref<number | null>(null);

    onMounted(async () => {
      await fetchCompanies();
    if (companies.value.length > 0) {
      selectedCompanyId.value = companies.value[0].companyId;
      fetchProducts();
    }
    });

    console.log("Products after fetch:", products);

    const navigateToAddProduct = () => {
        router.push('/products/add');
    };

    const navigateToEditProduct = (productId: Number) => {
        router.push(`/products/edit/${productId}`);
    };
    
    const fetchProducts = async () => {
      if (selectedCompanyId.value) {
      try {
        const response = await axios.get(`http://localhost:5176/Companies/${selectedCompanyId.value}/products`);
        products.value = response.data;
      } catch (error) {
        console.error("Error fetching products:", error);
      }
    }
    };
    const fetchCompanies = async () => {
    try {
      const response = await axios.get('http://localhost:5176/Companies/all');
      companies.value = response.data;
    } catch (error) {
      console.error("Error fetching companies:", error);
    }
    };
    const onCompanyChange = () => {
      fetchProducts();
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
    const getCompanyNameById = (companyId: number) => {
    const company = companies.value.find(c => c.companyId === companyId);
    return company ? company.name : 'Ettevõte ei leitud';
    };
</script>