const validationResult = require ("express-validator")
const loginService = require("../services/loginService")

const getPageLogin = (req, res) => {
    return res.render("login.ejs", {
        errors: req.flash("errors")
    })
}

const handleLogin = async (req, res) => {
    const errorsArr = []
    const validationErrors = validationResult(req)
    if (!validationErrors.isEmpty()) {
        const errors = Object.values(validationErrors.mapped())
        errors.forEach((item) => {
            errorsArr.push(item.msg)
        })
        req.flash("errors", errorsArr)
        return res.redirect("/login")
    }

    try {
        await loginService.handleLogin(req.body.email, req.body.password)
        return res.redirect("/")
    } catch (err) {
        req.flash("errors", err)
        return res.redirect("/login")
    }
}

const checkLoggedIn = (req, res, next) => {
    if (!req.isAuthenticated()) {
        return res.redirect("/login")
    }
    next()
}

const checkLoggedOut = (req, res, next) => {
    if (req.isAuthenticated()) {
        return res.redirect("/")
    }
    next()
}

const postLogOut = (req, res) => {
    req.session.destroy(function(err) {
      return res.redirect("/login")    
    })
}

module.exports = {
    getPageLogin: getPageLogin,
    handleLogin: handleLogin,
    checkLoggedIn: checkLoggedIn,
    checkLoggedOut: checkLoggedOut,
    postLogOut: postLogOut
}