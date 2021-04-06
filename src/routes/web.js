const express = require("express")
const homePageController = require ( "../controllers/homePageController")
const registerController = require ( "../controllers/registerController")
const loginController = require ( "../controllers/loginController")
const auth = require ( "../validation/authValidation")
const passport = require ( "passport")
const initPassportLocal = require ( "../controllers/passportLocalController")
const depotController = require("../controllers/depotController")
const historiqueTransfertsController = require("../controllers/historiqueTransfertsController")

// Initialise tous les `passports` relatifs à l'authentification
initPassportLocal()

const router = express.Router()

const initWebRoutes = (app) => {
    router.get("/", loginController.checkLoggedIn, homePageController.handleHelloWorld)
    router.get("/login",loginController.checkLoggedOut, loginController.getPageLogin)
    router.post("/login", passport.authenticate("local", {
        successRedirect: "/",
        failureRedirect: "/login",
        successFlash: true,
        failureFlash: true
    }))

    router.get("/register", registerController.getPageRegister)
    router.post("/register", auth.validateRegister, registerController.createNewUser)
    router.post("/logout", loginController.postLogOut)
    router.post("/carte",loginController.checkLoggedIn, homePageController.activerDesactiverCarte)
    router.post("/transfert", loginController.checkLoggedIn, depotController.transfert)
    router.get ("/historique", loginController.checkLoggedIn, historiqueTransfertsController.getPagehistroriqueTransfert)
    router.get("/transfert", loginController.checkLoggedIn, depotController.getpageTransfert)
    router.get("**", (req,res)=>{
        res.redirect("/login")
    } )
    return app.use("/", router)
}
module.exports = initWebRoutes
