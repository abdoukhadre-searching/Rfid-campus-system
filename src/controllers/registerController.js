import registerService from "./../services/registerService";
import { validationResult } from "express-validator";

const getPageRegister = (req, res) => {
    return res.render("register.ejs", {
        errors: req.flash("errors")
    });
};

const createNewUser = async (req, res) => {
    //valider les champs requises
    const errorsArr = [];
    const validationErrors = validationResult(req);
    if (!validationErrors.isEmpty()) {
        const errors = Object.values(validationErrors.mapped());
        errors.forEach((item) => {
            errorsArr.push(item.msg);
        });
        req.flash("errors", errorsArr);
        return res.redirect("/register");
    }
    //Créer un nouveau utilisateur
    const newUser = {
        codePermanent: req.body.codePermanent,
        email: req.body.email,
        password: req.body.password
    };
    //const msg = "Compte crée avec succés ";
    try {
        await registerService.createNewUser(newUser);
        return res.redirect("/login");
    } catch (err) {
        req.flash("errors", err);
        return res.redirect("/register");
    }
};
module.exports = {
    getPageRegister: getPageRegister,
    createNewUser: createNewUser
};