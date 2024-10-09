export type Invoice = {
    title: string
    address: string
    city: string
    zipCode: number
    country: string
    invoiceNr: number
    dateCreated: Date
    dateDue: Date
    condition: string
    delayFine: string
}