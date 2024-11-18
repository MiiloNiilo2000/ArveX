import type { Company } from "../types/company";
import { defineStore } from 'pinia';
import { ref } from 'vue';

export const useCompanyStore = defineStore('company', () => { 
  const companies = ref<Company[]>([]);

  const deleteCompany= (id: number) => {
    const index = companies.value.findIndex((company) => company.companyId === id);
    if (index !== -1) {
      companies.value.splice(index, 1);
    }
  };
  const editCompany = (updatedCompany: Company) => {
    const index = companies.value.findIndex((company) => company.companyId === updatedCompany.companyId);
    if (index !== -1) {
        companies.value[index] = { ...updatedCompany };
    }
};

const getCompanyById = (Id: number): Company | undefined => {
    return companies.value.find(company => company.companyId === Id);
};

  return { companies, deleteCompany, editCompany, getCompanyById };
});