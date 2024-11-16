<template>
  <div>
    <UTable :columns="columns" :rows="invoices">
      <template #title-data="{ row }">
        <span class="text-emerald-500 font-bold">
          {{ row.title }}
        </span>
      </template>

      <template #invoiceNumber-data="{ row }">
        <span class="text-indigo-400">
          {{ row.invoiceNumber }}
        </span>
      </template>

      <template #dateCreated-data="{ row }">
        <span class="text-gray-400">
          {{ new Date(row.dateCreated).toLocaleDateString() }}
        </span>
      </template>

      <template #dateDue-data="{ row }">
        <span :class="{'text-red-500 font-bold': isPastDue(row.dateDue), 'text-green-500': !isPastDue(row.dateDue)}">
          {{ new Date(row.dateDue).toLocaleDateString() }}
        </span>
      </template>

      <template #delete-data="{ row }">
        <UButton 
          @click="deleteInvoice(row.invoiceId)" 
          color="gray" 
          variant="ghost" 
          icon="i-heroicons-trash" 
        />
      </template>

      <template #view-data="{ row }">
        <UButton 
          @click="viewInvoice(row)" 
          color="gray" 
          variant="ghost" 
          icon="i-heroicons-eye" 
        />
      </template>
    </UTable>
  </div>
</template>
  
<script setup lang="ts">
  import { ref, onMounted } from 'vue';
  import { generateInvoicePDF } from '../stores/invoiceStores'; 
  import type { Invoice } from "../types/invoice";
  import { useApi } from '../composables/useApi';

  const invoices = ref<Invoice[]>([]);
    const { customFetch } = useApi();

  const columns = ref([
    { key: 'title', label: 'Firma Nimi' },
    { key: 'invoiceNumber', label: 'Arve Number' },
    { key: 'dateCreated', label: 'Loomiskuupäev' },
    { key: 'dateDue', label: 'Tähtaeg' },
    { key: 'delete', label: 'Kustuta' },
    { key: 'view', label: 'Vaata' },
  ]);

  const isPastDue = (dueDate: string): boolean => {
    return new Date(dueDate) < new Date();
  };

  const fetchInvoices = async () => {
    try {
      const response = await customFetch<Invoice[]>(`InvoiceHistory/all`, { method: 'GET' });
      invoices.value = response; 
    } catch (error) {
      console.error("Error fetching invoices:", error);
    }
  };

  const deleteInvoice = async (id: number) => {
    try {
      await customFetch<Invoice[]>(`InvoiceHistory/${id}`, { method: 'DELETE' });
      invoices.value = invoices.value.filter(invoice => invoice.invoiceId !== id);
    } 
    catch (error) {
      console.error("Error deleting invoice:", error);
    }
  };

  const viewInvoice = async (row: any) => {
    const state = {
      title: row.title,
      clientRegNr: row.clientRegNr,
      clientKMKR: row.clientKMKR,
      address: row.address,
      zipCode: row.zipCode,
      country: row.country,
      invoiceNumber: row.invoiceNumber,
      dateCreated: row.dateCreated,
      dateDue: row.dateDue,
      condition: row.condition,
      delayFine: row.delayFine,
      selectedFont: row.font,
      productsAndQuantities: row.productsAndQuantities 
    };

    generateInvoicePDF(state, "GeneratePdfWithoutSaving");
  };

  onMounted(() => {
    fetchInvoices();
  });
  </script>
  