<template>
    <UForm
      :validate="validate"
      :state="state"
      class="space-y-4"
      @submit.prevent="submitForm"
      @error="onError"
    >
      <UFormGroup label="Title" name="title">
        <UInput v-model="state.title" />
      </UFormGroup>
      <UFormGroup label="Address" name="address">
        <UInput v-model="state.address" />
      </UFormGroup>

      <UButton type="submit">Generate PDF</UButton>

    </UForm>
</template>

<script setup lang="ts">
    import { reactive } from 'vue';
    import type { FormError, FormErrorEvent, FormSubmitEvent } from "#ui/types";
    import { useInvoiceStore } from "~/stores/stores";
    import type { Invoice } from "~/types/invoice";
    import axios from 'axios';

    const state = reactive({
      title: '',
      address: '',
    });

    const validate = (state: any): FormError[] => {
      const errors = [];
      if (!state.title) errors.push({ path: "title", message: "Required" });
      if (!state.address) errors.push({ path: "address", message: "Required" });
      return errors;
    };
    
    const submitForm = async () => {
      console.log("Form submitted");
      try {
        const response = await axios.post('http://localhost:5176/CreateInvoice', { title: state.title, address: state.address }, { responseType: 'blob' });

        const url = window.URL.createObjectURL(new Blob([response.data]));
        const link = document.createElement('a');
        link.href = url;
        link.setAttribute('download', 'invoice.pdf');
        document.body.appendChild(link);
        link.click();
        link.remove();
      } catch (error) {
        console.error("Error generating PDF:", error);

      }
    };

    async function onError(event: FormErrorEvent) {
      const element = document.getElementById(event.errors[0].id);
      element?.focus();
      element?.scrollIntoView({ behavior: "smooth", block: "center" });
    }

  </script>