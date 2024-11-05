<template>
    <div>
      <UTable :columns="columns" :rows="invoices">
        <template #delete-data="{ row }">
                <UButton @click="deleteInvoice(row.invoiceId)" color="gray" variant="ghost" icon="mdi-delete" />
        </template>
        <template #view-data="{ row }">
                <UButton @click="viewInvoice(row)" color="gray" variant="ghost" icon="mdi-eye" /> 
        </template>
      </UTable>
    </div>
  </template>
  
  <script setup>
  import { ref, onMounted } from 'vue';
  import axios from 'axios';
  import { generateInvoicePDF } from '../stores/invoiceUtils'; // ARVE VAATAMINE LISAB SELLE UUESTI DATABAASI, KORDA TEHA //

  const invoices = ref([]);

  const columns = ref([
    { key: 'title', label: 'Firma Nimi' },
    { key: 'invoiceNumber', label: 'Arve Number' },
    { key: 'dateCreated', label: 'Loomiskuupäev' },
    { key: 'dateDue', label: 'Tähtaeg' },
    { key: 'delete', label: 'Kustuta' },
    { key: 'view', label: 'Vaata' },
  ]);

  const fetchInvoices = async () => {
    try {
      const response = await axios.get('http://localhost:5176/InvoiceHistory/all');
      invoices.value = response.data; 
    } catch (error) {
      console.error("Error fetching invoices:", error);
    }
  };

  const deleteInvoice = async (id) => {
    try {
      await axios.delete(`http://localhost:5176/InvoiceHistory/${id}`);

      invoices.value = invoices.value.filter(invoice => invoice.invoiceId !== id);

      if (invoices.value.length === 0) {
        console.log("No more invoices left.");
      }
    } 
    catch (error) {
      console.error("Error deleting invoice:", error);
    }
  };

  const viewInvoice = async (row) => {
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
      productIds: row.productIds 
    };

    generateInvoicePDF(state);
  };

  onMounted(() => {
    fetchInvoices();
  });
  </script>
  