<template>
    <div class="flex justify-center items-center pt-9">
        <div class="w-96 p-6 flex flex-col h-99">
            <h2 class="text-2xl font-bold mb-4 text-center">Lisa toode</h2>
    <UForm
      :validate="validate"
      :state="state"
      class="space-y-4 "
      @submit="onSubmit"
      @error="onError"
    >
    <UFormGroup label="Toote nimi" name="name">
        <UInput v-model="state.name" />
      </UFormGroup>
      <UFormGroup label="Kirjeldus" name="description">
        <UInput v-model="state.description" />
      </UFormGroup>
      <UFormGroup label="Hind" name="price">
        <UInput v-model="state.price" />
      </UFormGroup>
      <UFormGroup label="Firma" name="companyId">
        <select v-model="state.companyId" class="w-full border p-2 rounded">
            <option value="" disabled>Select a company</option>
            <option v-for="company in companies" :key="company.companyId" :value="company.companyId">
              {{ company.name }}
            </option>
          </select>
      </UFormGroup>
  
      <UButton type="submit"> Lisa </UButton>
    </UForm>
    </div>
    </div>
  </template>

<script setup lang="ts">
import type { FormError, FormErrorEvent, FormSubmitEvent } from "#ui/types";
import type { Product } from "../types/product";
import { reactive, ref, onMounted, watch } from 'vue';
import axios from "axios";
import { useRoute, useRouter } from 'vue-router';

const route = useRoute();
const router = useRouter();

const state = reactive<Product>({
    productId: 0,
    name: '',
    description: '',
    price: null,
    companyId: null
  });

const companies = ref<{ companyId: number; name: string }[]>([]);

const fetchCompanies = async () => {
  try {
    const response = await axios.get('http://localhost:5176/Companies/all');
    companies.value = response.data;
    console.log('Fetched companies:', companies.value);
  } catch (error) {
    console.error("Error fetching companies:", error);
  }
};

onMounted(fetchCompanies);
watch(() => state.companyId, (newVal) => {
  console.log('Selected companyId:', newVal);
});

const addProduct = async (product) => {
      try {
          await axios.post('http://localhost:5176/Products', product);
      } catch (error) {
          console.error("Error adding product:", error);
      }
  };

  const validate = (state: any): FormError[] => {
    const errors = [];
    if (!state.name) errors.push({ path: "name", message: "Required" });
    if (!state.description) errors.push({ path: "description", message: "Required" });
    if (!state.price) errors.push({ path: "price", message: "Required" });
    if (!state.companyId) errors.push({ path: "companyId", message: "Required" });
    return errors;
  };

  async function onSubmit(event: FormSubmitEvent<any>) {
    addProduct({ ...state });
    await router.push("/products");
  }

  async function onError(event: FormErrorEvent) {
    const element = document.getElementById(event.errors[0].id);
    element?.focus();
    element?.scrollIntoView({ behavior: "smooth", block: "center" });
  }
  </script>