<template>
  <div>

    <input 
      type="text"
      v-model="searchTerm"
      placeholder="Otsi..."
      class="form-search"
      />

    <UTable :columns="columns" :rows="filteredInvoices">

      <template #header="{ column }">
        <span @click="toggleSort(column)" :class="{ 'cursor-pointer': column.sortable }">
          {{ column.label }}
          <span v-if="sortState.key === column.key">
            {{ sortState.order === 'asc' ? '▲' : '▼' }}
          </span>
        </span>
      </template>

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

      <template #view-data="{ row }">
        <UButton 
        @click="viewInvoice(row)" 
        color="gray" 
        variant="ghost" 
        icon="i-heroicons-arrow-down-tray" 
        />
      </template>

      <template #delete-data="{ row }">
        <UButton 
          @click="deleteInvoice(row.invoiceId)" 
          color="gray" 
          variant="ghost" 
          icon="i-heroicons-trash" 
        />
      </template>

      
    </UTable>
  </div>
</template>
  
<script setup lang="ts">
  import { ref, onMounted, computed } from 'vue';
  import { generateInvoicePDF } from '../stores/invoiceStores'; 
  import type { Invoice } from "../types/invoice";
  import { useApi } from '../composables/useApi';

  const invoices = ref<Invoice[]>([]);
    const { customFetch } = useApi();

  const columns = ref([
    { key: 'title', label: 'Firma Nimi', sortable: true },
    { key: 'invoiceNumber', label: 'Arve Number', sortable: true },
    { key: 'dateCreated', label: 'Loomiskuupäev', sortable: true },
    { key: 'dateDue', label: 'Tähtaeg', sortable: true },
    { key: 'view', label: 'Laadi Alla' },
    { key: 'delete', label: 'Kustuta' },
  ]);

  const searchTerm = ref('');

  const filteredInvoices = computed(() => {
    if (!searchTerm.value) return invoices.value;

    const query = searchTerm.value.toLowerCase();
    return invoices.value.filter(invoice => {
      return (
        invoice.title.toLowerCase().includes(query) ||
        invoice.invoiceNumber.toString().includes(query) ||
        invoice.clientKMKR.toLowerCase().includes(query) ||
        invoice.clientRegNr.toLowerCase().includes(query) ||
        invoice.country.toLowerCase().includes(query) ||
        new Date(invoice.dateCreated).toLocaleDateString().includes(query) ||
        new Date(invoice.dateDue).toLocaleDateString().includes(query)
      );
    });
  });


  const sortState = ref<{ key: keyof Invoice | null; order: 'asc' | 'desc' }>({
    key: null,
    order: 'asc',
  });

  type Column = {
    key: keyof Invoice | string;
    label: string;
    sortable?: boolean;
  }

  const sortedInvoices = computed(() => {
    if (!sortState.value.key) return  invoices.value;
     
    const key = sortState.value.key;
    const order = sortState.value.order === 'asc' ? 1 : -1;

    return [...invoices.value].sort((a, b) => {
      const aValue = a[key];
      const bValue = b[key];

      if (aValue < bValue) return -1 * order;
      if (aValue > bValue) return 1 * order;
      return 0;
    });
  });

  const toggleSort = (column: Column) => {
    if (!column.sortable) return;

    if (sortState.value.key === column.key) {
      sortState.value.order = sortState.value.order === 'asc' ? 'desc' : 'asc';
    } else {
      sortState.value.key = column.key as keyof Invoice;
      sortState.value.order = 'asc';
    }
  };

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
  
<style scoped>
  .form-search {
  border: 2px solid #38a169; 
  background-color: #121212;
  color: rgb(255, 255, 255);
  border-radius: 0.375rem;
  padding: 0.375rem 0.75rem;
  font-size: 1rem;
  width: auto;
  height: 2.5rem; 
  transition: border-color 0.2s ease-in-out;
  }

  .form-search:focus {
    border-color: #38a169; 
    outline: none;
    box-shadow: 0 0 0 0.1rem #357955;
  }
</style>