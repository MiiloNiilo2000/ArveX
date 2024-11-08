<template>
    <div class="flex items-center justify-center min-h-screen">
      <div class="container">  
        
        <div class="top-text">
            Arve<span class="last-letter">X</span>
            <div class="gradient-line"></div>
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

        <NuxtLink to="/products"
            class="route-buttons-base product-button text-white 
            bg-gradient-to-r from-green-400 via-green-500 to-green-600 
            hover:bg-gradient-to-br 
            focus:ring-4 
            focus:outline-none 
            focus:ring-green-300 
            dark:focus:ring-green-800 shadow-lg shadow-green-500/50 
            dark:shadow-lg 
            dark:shadow-green-800/80 
            font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 mb-2">
            Halda tooteid 
            <div class="emoji-text">
                📦
            </div> 
        </NuxtLink>

        <NuxtLink to="/products"
            class="route-buttons-base history-button text-white 
            bg-gradient-to-r from-green-400 via-green-500 to-green-600 
            hover:bg-gradient-to-br 
            focus:ring-4 
            focus:outline-none 
            focus:ring-green-300 
            dark:focus:ring-green-800 shadow-lg shadow-green-500/50 
            dark:shadow-lg 
            dark:shadow-green-800/80 
            font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 mb-2">
            Vaata ajalugu
            <div class="emoji-text">
                📜
            </div> 
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
    text-shadow: 0 0 4px black;
}

.centered-button {
    position: absolute;
    width: 80%;
    top: 220%;
    left: 50%;
    transform: translateX(-50%);
    font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    text-shadow: 0 0 4px black;
}

.icon-button-1 {
    color: black;
    background-color: transparent;
    border: none;
}

.icon-button-2 {
    position: absolute;
    top: 310%;
    right: -80%;
}

.route-buttons-base {
    position: absolute;
    font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    color: white;
    font-size: large;
    text-shadow: 0 0 4px black;
    user-select: none;
    width: 70%; 
    height: 50%;
}
.product-button{
    top: 290%; 
    left: -100%;
}

.history-button{
    top: 290%;
    right: -100%;
}

.login-text{
    position: absolute;
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

.emoji-text{
    padding: 7%;
    font-size:xx-large;
    user-select: none;
    text-shadow: 0 0 6px black;
}

.gradient-line {
    position: absolute;
    top: 75%;
    background: linear-gradient(to right, #33cd35 60%, #9d00ff);
    height: 2px; /* line thickness */
    width: 100%; /* line length */
    border-radius: 4px;
}

@font-face {
font-family: 'LowBallItalicBold';
src: url('/assetsFront/fonts/LowballNeueBoldItalic-lgXJw.ttf') format('truetype');
font-weight: bold;
font-style: normal;
}

</style>
  
  