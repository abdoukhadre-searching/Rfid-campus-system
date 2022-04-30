import passportLocal from "passport-local"
import passport from "passport"
import {findUserByEmail, comparePassword, findUserById } from "../services/loginService.js"

const LocalStrategy = passportLocal.Strategy

export const initPassportLocal = () => {
    passport.use(new LocalStrategy({
            usernameField: 'email',
            passwordField: 'password',
            passReqToCallback: true
        },
        async (req, email, password, done) => {
            try {
                await findUserByEmail(email).then(async (user) => {
                    if (!user) {
                        return done(null, false, req.flash("errors", `L'email "${email}" n'existe pas`))
                    }
                    if (user) {
                        const match = await comparePassword(password, user)
                        if (match === true) {
                            console.log(user)
                            return done(null, user, null)
                        } else {
                            return done(null, false, req.flash("errors", match)
                            )
                        }
                    }
                })
            } catch (err) {
                console.log(err)
                return done(null, false, { message: err })
            }
        }))

}

passport.serializeUser((user, done) => {
    done(null, user.idCompteEtudiant) //probleme a résoudre , but is finally resolved 
})

passport.deserializeUser((id, done) => {
    findUserById(id).then((user) => {
        return done(null, user)
    }).catch(error => {
        return done(error, null)
    })
})

// module.exports = initPassportLocal