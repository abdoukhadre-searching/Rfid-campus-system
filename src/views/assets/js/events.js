var repas = document.getElementById("repas");
var dej = document.getElementById("petit_dej");
var jour = document.getElementById("valeurJour");
var semaine = document.getElementById("valeurSemaine");
var mois = document.getElementById("valeurMois");
var somme = document.getElementById("somme");

repas.addEventListener("click", ajouter);
function ajouter(){
    var r = parseInt(repas.value);
    somme.innerHTML += r;
}