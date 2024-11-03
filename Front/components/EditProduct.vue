<template>
    <div class="flex justify-center items-center pt-9">
      <div class="w-96 p-6 flex flex-col h-99">
        <h2 class="text-2xl font-bold mb-4 text-center">Muuda toode</h2>
        <UForm
          :validate="validate"
          :state="state"
          class="space-y-4"
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
            <UInput v-model="state.companyId" />
          </UFormGroup>
          <UButton type="submit"> Salvesta </UButton>
        </UForm>
      </div>
    </div>
  </template>

<script setup lang="ts">
  import type { FormError, FormErrorEvent, FormSubmitEvent } from "#ui/types";
  import type { Product } from "../types/product";
  import { reactive, onMounted } from 'vue';
  import { useRoute, useRouter } from 'vue-router';
  import axios from "axios";

  const state = reactive<Product>({
      productId: 0,
      name: '',
      description: '',
      price: 0,
      companyId: 0
  });

  const editProduct = async (product: Product) => {
    try {
        console.log("Updating product:", product);
        
        const response = await axios.put(`http://localhost:5176/Products/${product.productId}`, product);

        console.log("Product updated successfully:", response.data);
        await router.push("/products");
    } catch (error) {
        console.error("Error updating product:", error);

        if (error.response) {
            console.error("Response data:", error.response.data);
            console.error("Response status:", error.response.status);
            console.error("Response headers:", error.response.headers);
        } else {
            console.error("Error message:", error.message);
        }
    }
};

  const route = useRoute();
  const router = useRouter();
  const productId = Number(route.params.id);

  const fetchProduct = async (id: number) => {
    try {
        const response = await axios.get(`http://localhost:5176/Products/${id}`);
        const product = response.data;
        if (product) {
            Object.assign(state, product);
        }
    } catch (error) {
        console.error("Error fetching product:", error);
    }
  };

  onMounted(async () => {
    await fetchProduct(productId);
  });

  const validate = (state: any): FormError[] => {
    const errors = [];
    if (!state.name) errors.push({ path: "name", message: "Required" });
    if (!state.description) errors.push({ path: "description", message: "Required" });
    if (!state.price) errors.push({ path: "price", message: "Required" });
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

</script>