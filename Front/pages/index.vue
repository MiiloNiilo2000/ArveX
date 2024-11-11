<template>
  <div class="bg-gray-100 min-h-screen">
    <section class="bg-blue-600 text-white py-20">
      <div class="container mx-auto px-6 text-center">
        <h1 class="text-4xl font-bold mb-4">ArveX</h1>
        <p class="text-lg mb-8">Loo arveid kiiresti ja mugavalt</p>
        <NuxtLink
          to="/create-invoice"
          class="bg-white text-blue-600 px-6 py-3 rounded-lg shadow-md font-semibold hover:bg-gray-100"
        >
          Proovi kohe
        </NuxtLink>
      </div>
    </section>

    <div class="absolute top-20 right-6">
      <NuxtLink
        to="/login"
        class="bg-white text-blue-600 px-6 py-3 rounded-lg shadow-md font-semibold hover:bg-gray-100"
      >
        Logi sisse
      </NuxtLink>
    </div>

    <section class="container mx-auto px-6 py-16 text-gray-700">
      <h2 class="text-3xl font-bold text-center mb-12">Miks valida ArveX?</h2>
      <div class="grid grid-cols-1 md:grid-cols-3 gap-8">
        <div class="bg-white p-6 rounded-lg shadow-md">
          <div class="text-blue-600 text-4xl mb-4 text-center">📑</div>
          <h3 class="text-xl font-semibold mb-2 text-center">Lihtne luua</h3>
          <p class="text-gray-600 text-center">Loo ja kohanda arveid vaid mõne klikiga.</p>
        </div>

        <div class="bg-white p-6 rounded-lg shadow-md">
          <div class="text-blue-600 text-4xl mb-4 text-center">📈</div>
          <h3 class="text-xl font-semibold mb-2 text-center">Jälgi arveid</h3>
          <p class="text-gray-600 text-center">Loodud arved salvestuvad, et saaksid neid hiljem hallata.</p>
        </div>

        <div class="bg-white p-6 rounded-lg shadow-md">
          <div class="text-blue-600 text-4xl mb-4 text-center">💼</div>
          <h3 class="text-xl font-semibold mb-2 text-center">Toote- / teenusebaasi lisamine</h3>
          <p class="text-gray-600 text-center">Loo oma toodetest ja / või teenustest loend ning lisa neid hõlbsalt arvele.</p>
        </div>
      </div>
    </section>

    <section class="bg-gray-200 py-16">
      <div class="container mx-auto px-6">
        <h2 class="text-3xl font-bold text-center mb-12">Kuidas luua?</h2>
        <div class="grid grid-cols-1 md:grid-cols-3 gap-8">
          <div class="bg-white p-6 rounded-lg shadow-md text-center">
            <div class="text-blue-600 text-4xl mb-4">1️⃣</div>
            <h3 class="text-xl font-semibold mb-2">Loo mall</h3>
            <p class="text-gray-600">Kui sulle ei meeldi meie pakutud arve kujundus, saad selle teha täpselt selliseks nagu soovid.</p>
          </div>
          <div class="bg-white p-6 rounded-lg shadow-md text-center">
            <div class="text-blue-600 text-4xl mb-4">2️⃣</div>
            <h3 class="text-xl font-semibold mb-2">Lisa firma ja tooted</h3>
            <p class="text-gray-600">Lisa arvele oma tooted ja teenused. Kui teed arvet firmale, pead sisestama vaid firma nime, ülejäänu jäta meie hooleks.</p>
          </div>
          <div class="bg-white p-6 rounded-lg shadow-md text-center">
            <div class="text-blue-600 text-4xl mb-4">3️⃣</div>
            <h3 class="text-xl font-semibold mb-2">Salvesta ja lae alla</h3>
            <p class="text-gray-600">Lae oma koostatud arve alla ja ongi valmis. Lisaks salvestuvad sinu koostatud arved sinu profiilile.</p>
          </div>
        </div>
      </div>
    </section>

    <section class="bg-blue-600 text-white py-16">
      <div class="container mx-auto px-6 text-center">
        <h2 class="text-3xl font-bold mb-6">Loo arveid juba täna</h2>
        <p class="text-lg mb-8">Registreeru ja loo arveid nagu professionaal.</p>
        <NuxtLink
          to="/signup"
          class="bg-white text-blue-600 px-6 py-3 rounded-lg shadow-md font-semibold hover:bg-gray-100"
        >
          Registreeri
        </NuxtLink>
      </div>
    </section>
  </div>
</template>

<script setup>
import { NuxtLink } from "#components";
</script>

<style scoped>
.container {
  max-width: 1200px;
}

section {
  position: relative;
}

.absolute {
  position: absolute;
}
</style>
