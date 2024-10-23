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
          <UButton type="submit"> Salvesta </UButton>
        </UForm>
      </div>
    </div>
  </template>

<script setup lang="ts">
  import type { FormError, FormErrorEvent, FormSubmitEvent } from "#ui/types";
  import type { Product } from "~/types/product";
  import { reactive, onMounted } from 'vue';
  import { useProductStore } from '~/stores/productStores';
  import { useRoute } from 'vue-router';


  const{editProduct, getProductById} = useProductStore();

  const state = reactive<Product>({
      id: 0,
      name: '',
      description: '',
      price: 0,
    });


  const route = useRoute();
  const productId = Number(route.params.id);


  onMounted(async () => {
    const product = await getProductById(productId); 
    if (product) {
      Object.assign(state, product); 
    }
  });

  const validate = (state: any): FormError[] => {
    const errors = [];
    if (!state.name) errors.push({ path: "name", message: "Required" });
    if (!state.description) errors.push({ path: "description", message: "Required" });
    if (!state.price) errors.push({ path: "price", message: "Required" });
    return errors;
  };

  async function onSubmit(event: FormSubmitEvent<any>) {
    console.log("Submitting product:", { ...state });
    editProduct({ ...state });
    await navigateTo("/products");
  }

  async function onError(event: FormErrorEvent) {
    const element = document.getElementById(event.errors[0].id);
    element?.focus();
    element?.scrollIntoView({ behavior: "smooth", block: "center" });
  }

</script>