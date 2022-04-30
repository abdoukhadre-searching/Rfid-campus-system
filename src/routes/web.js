import express from "express"
import  * as homePageController from "../controllers/homePageController.js"
import { getPageRegister,createNewUser } from "../controllers/registerController.js"
import {postLogOut, getPageLogin, checkLoggedIn, handleLogin, checkLoggedOut } from  "../controllers/loginController.js"
import {validateRegister} from "../validation/authValidation.js"
import passport from "passport"
import  {initPassportLocal} from "../controllers/passportLocalController.js"
import {getpageTransfert, transfert } from "../controllers/depotController.js"
import  {getPagehistroriqueTransfert} from "../controllers/historiqueTransfertsController.js"
import  {achatPage} from "../controllers/achatController.js"
import {achatHistorique} from "../controllers/historiqueAchatContoller.js"

// Initialise tous les `passports` relatifs à l'authentification
initPassportLocal()

const router = express.Router()

export const initWebRoutes = (app) => {
    router.get("/", checkLoggedIn, homePageController.handleHelloWorld)
    router.get("/login",checkLoggedOut, getPageLogin)
    router.post("/login", passport.authenticate("local", {
        successRedirect: "/",
        failureRedirect: "/login",
        successFlash: true,
        failureFlash: true
    }))

    router.get("/register", getPageRegister)
    router.post("/register", validateRegister, createNewUser)
    router.post("/logout", postLogOut)
    router.post("/carte",checkLoggedIn, homePageController.activerDesactiverCarte)
    router.post("/transfert", checkLoggedIn, transfert)
    router.get ("/historique", checkLoggedIn, getPagehistroriqueTransfert)
    router.get("/historique-achat", checkLoggedIn, achatHistorique)
    router.get("/transfert", checkLoggedIn, getpageTransfert)
    router.get("/achat", checkLoggedIn, achatPage)
    router.get("**", (req,res)=>{
        res.redirect("/login")
    } )
    return app.use("/", router)
}
// module.exports = initWebRoutes
