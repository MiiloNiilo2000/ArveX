<template>
    <div class="flex items-center justify-center min-h-screen">
      <div class="container">

         

        
        
        

        

        
        
        <div class="top-text">
            Arve<span class="last-letter">X</span>
            <NuxtLink
            to="/create-invoice"
            class="centered-top-button text-white 
            bg-gradient-to-r from-green-400 via-green-500 to-green-600 
            hover:bg-gradient-to-br 
            focus:ring-4 
            focus:outline-none 
            focus:ring-green-300 
            dark:focus:ring-green-800 shadow-lg shadow-green-500/50 
            dark:shadow-lg 
            dark:shadow-green-800/80 
            font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 mb-2">
            Alusta uue arve loomist
        </NuxtLink>  

        <div class="login-text">
            Kõikide funktsioonide kasutamiseks peate olema sisse logitud
        </div>

        <NuxtLink
            to="/signup"
            class="centered-button text-white 
            bg-gradient-to-r from-green-400 via-green-500 to-green-600 
            hover:bg-gradient-to-br 
            focus:ring-4 
            focus:outline-none 
            focus:ring-green-300 
            dark:focus:ring-green-800 shadow-lg shadow-green-500/50 
            dark:shadow-lg 
            dark:shadow-green-800/80 
            font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 mb-2">
            Logi sisse / Registreeri
        </NuxtLink>   

        <NuxtLink to="/products">
            <div class="product-text">
                Halda tooteid
            </div>

            <UButton class="icon-button icon-button-1" icon="mdi-box-variant" />
        </NuxtLink>

        <NuxtLink to="/view-history">
            <div class="history-text">
                Vaata ajalugu
            </div>

            <UButton class="icon-button icon-button-2" icon="mdi-eye" />
        </NuxtLink>

        </div>

    
    
    </div>

    </div>
  </template>
  
  <script>
    import { NuxtLink } from "#components";
  export default {
    name: "Hexagon",
  };

  </script>
  
<style scoped>

.container {
    display: flex;
    flex-direction: column;
    align-items: center;
    min-height: 5vh;
    position: relative;
}

.top-text {
    font-size: 100px;
    position: absolute;
    top: -50vh;
    left: 50%;
    transform: translateX(-50%);
    font-family: 'LowBallItalicBold';
    color: #33cd35;
    user-select: none;
}

.last-letter {
    color: #9d00ff; 
    text-shadow: 0 0 8px #9d00ff;
}

.centered-top-button {
    position: absolute;
    top: 80%;
    width: 80%;
    left: 50%;
    transform: translateX(-50%);
    font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
}

.centered-button {
    position: absolute;
    width: 80%;
    top: 220%;
    left: 50%;
    transform: translateX(-50%);
    font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
}

.icon-button-base {
    color: none;
    background: #38a169;
    border: none;
    cursor: pointer;
    border-radius: 50%; 
    padding: 10px; 
    transition: box-shadow 0.2s, transform 0.2s;
}

.icon-button:hover {
    box-shadow: 0 4px 8px #38a169;
}

.icon-button-1 {
    position: absolute;
    top: 310%;
    left: -85%;

}

.icon-button-2 {
    position: absolute;
    top: 310%;
    right: -80%;
}

.product-text{
    position: absolute;
    top: 290%; 
    left: -100%;
    font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    color: white;
    font-size: large;
    user-select: none;
}

.history-text{
    position: absolute;
    top: 290%;
    width: 50%; 
    right: -100%;
    font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    color: white;
    font-size: large;
    user-select: none;
}

.login-text{
    position: fixed;
    text-align: center;
    top: 180%;
    width: 130%; 
    left: 50%;
    transform: translateX(-50%);
    font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    font-size: large;
    color: white;
    user-select: none;
}

@font-face {
font-family: 'LowBallItalicBold';
src: url('/assetsFront/fonts/LowballNeueBoldItalic-lgXJw.ttf') format('truetype');
font-weight: bold;
font-style: normal;
}

</style>
  
  