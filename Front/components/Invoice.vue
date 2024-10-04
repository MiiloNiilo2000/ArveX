<template>
    <UForm
      :validate="validate"
      :state="state"
      class="space-y-4"
      @error="onError"
    >
      <UFormGroup label="Title" name="title">
        <UInput v-model="state.title" />
      </UFormGroup>

    </UForm>
</template>

<script setup lang="ts">
    import { reactive } from 'vue';
    import type { FormError, FormErrorEvent, FormSubmitEvent } from "#ui/types";
    import { useInvoiceStore } from "~/stores/stores";
    import type { Invoice } from "~/types/invoice";

    const state = reactive<Omit<Invoice, 'id'>>({
      title: '',
    });

    const validate = (state: any): FormError[] => {
      const errors = [];
      if (!state.title) errors.push({ path: "title", message: "Required" });
      return errors;
    };
    
    async function onError(event: FormErrorEvent) {
      const element = document.getElementById(event.errors[0].id);
      element?.focus();
      element?.scrollIntoView({ behavior: "smooth", block: "center" });
    }

  </script>