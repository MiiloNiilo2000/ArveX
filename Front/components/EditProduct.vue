<template>
  <div class="flex justify-center items-center pt-9">
      <div class="w-96 p-6 flex flex-col h-99">
          <h2 class="text-2xl font-bold mb-4 text-center">Muuda toodet</h2>
      <UForm
        :validate="validate"
        :state="state"
        class="space-y-4"
        @submit="onSubmit"
        @error="onError"
      >
        <UFormGroup label="Toote nimi" name="name">
          <UInput v-model="state.name" color="emerald" class="bg-gray-900 rounded-md"/>
        </UFormGroup>
        <UFormGroup label="Kirjeldus" name="description">
          <UInput v-model="state.description" color="emerald" class="bg-gray-900 rounded-md"/>
        </UFormGroup>
        <UFormGroup label="Hind" name="price">
          <UInput v-model="state.price" color="emerald" class="bg-gray-900 rounded-md"/>
        </UFormGroup>
        <UFormGroup label="Maksuprotsent" name="taxPercent">
          <UInput v-model="state.taxPercent" color="emerald" class="bg-gray-900 rounded-md"/>
        </UFormGroup>
        <UFormGroup label="Firma" name="companyId">
        <select v-model="state.companyId" class="w-full ">
            <option value="" disabled>Vali ettevõte:</option>
            <option v-for="company in companies" :key="company.companyId" :value="company.companyId">
              {{ company.name }}
            </option>
          </select>
      </UFormGroup>
      <div class="col-span-2 flex justify-center">
      <UButton type="submit"> Salvesta </UButton>
      </div>
      </UForm>
    </div>
  </div>
</template>

<script setup lang="ts">
  import type { FormError, FormErrorEvent, FormSubmitEvent } from "#ui/types";
  import type { Product } from "../types/product";
  import { reactive, onMounted, ref } from 'vue';
  import { useRoute, useRouter } from 'vue-router';
  import { useApi } from '../composables/useApi';

  const { customFetch } = useApi(); 
  const companies = ref<{ companyId: number; name: string }[]>([]);
  const route = useRoute();
  const router = useRouter();
  const productId = Number(route.params.id);

  const state = reactive<Product>({
      productId: 0,
      name: '',
      description: '',
      price: 0,
      companyId: 0,
      taxPercent: 0
  });

  const fetchCompanies = async () => {
    try {
      const response = await customFetch<Company[]>(`Profile/Companies`, { method: 'GET' });
      companies.value = response;
    } catch (error) {
      console.error("Error fetching companies:", error);
    }
  };

  const editProduct = async (product: Product) => {
    try {
        await customFetch(`Products/${product.productId}`, {
            method: 'PUT',
            body: product,
        });
        await router.back();
    } catch (error) {
        console.error("Error updating product:", error);

    }
  };

  const fetchProduct = async (id: number) => {
    try {
        const response = await customFetch<Product[]>(`Products/${id}`, { method: 'GET' })
        const product = response;
        if (product) {
            Object.assign(state, product);
        }
    } catch (error) {
        console.error("Error fetching product:", error);
    }
  };

  const validate = (state: any): FormError[] => {
    const errors = [];
    if (!state.name) errors.push({ path: "name", message: "Required" });
    if (!state.description) errors.push({ path: "description", message: "Required" });
    if (!state.price) errors.push({ path: "price", message: "Required" });
    if (!state.taxPercent) errors.push({ path: "taxPercent", message: "Required" });
    if (!state.companyId) errors.push({ path: "companyId", message: "Required" });
    return errors;
  };

  async function onSubmit(event: FormSubmitEvent<any>) {
    console.log("Submitting product:", { ...state });
    await editProduct({ ...state });
  }

  async function onError(event: FormErrorEvent) {
    const element = document.getElementById(event.errors[0].id);
    element?.focus();
    element?.scrollIntoView({ behavior: "smooth", block: "center" });
  }

  onMounted(async () => {
    await fetchProduct(productId);
    await fetchCompanies();
  });
</script>

<style scoped>
select {
  border: 1.5px solid #38a169; 
  background-color:#111827; 
  color: white; 
  border-radius: 0.375rem; 
  padding: 0.2rem 0.75rem; 
  font-size: 1rem; 
  transition: border-color 0.2s ease-in-out; 
  }

  select:focus {
    border-color: #42ac4e; 
    outline: none; 
    box-shadow: 0 0 0 0.1rem rgba(66, 248, 56, 0.413); 
  }
  
  .form-field {
  border: 1px solid #38a169; 
  background-color: #121212;
  color: black;
  border-radius: 0.375rem;
  padding: 0.375rem 0.75rem;
  font-size: 1rem;
  width: 100%;
  height: 3rem; 
  transition: border-color 0.2s ease-in-out;
  }

  .form-field:focus {
    border-color: #2f855a; 
    outline: none;
    box-shadow: 0 0 0 0.2rem rgba(56, 189, 248, 0.25);
  }</style>