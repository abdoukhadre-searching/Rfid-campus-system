import {_createNewUser} from "./../services/registerService.js"
import { validationResult } from "express-validator"

export const getPageRegister = (req, res) => {
    return res.render("register.ejs", {
        errors: req.flash("errors")
    })
} 

export const createNewUser = async (req, res) => {
    //valider les champs requises
    const errorsArr = []
    const validationErrors = validationResult(req)
    if (!validationErrors.isEmpty()) {
        const errors = Object.values(validationErrors.mapped())
        errors.forEach((item) => {
            errorsArr.push(item.msg)
        })
        req.flash("errors", errorsArr)
        return res.redirect("/register")
    }
    //Créer un nouveau utilisateur
    const newUser = {
        codePermanent: req.body.codePermanent,
        email: req.body.email,
        //contact: req.body.contact,
        password: req.body.password
    }
    try {
        await createNewUser(newUser)
        return req.session.sessionFlash = {
            type: 'success',
            message: 'Compte crée avec succés ! Maintenant accéder à votre compte'
        },
        res.redirect("/login")
    } catch (err) {
        req.flash("errors", err)
        return res.redirect("/register")
    }
}
// module.exports = {
//     getPageRegister: getPageRegister,
//     createNewUser: createNewUser
// }