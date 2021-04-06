require('dotenv').config()
import express from "express"
import configViewEngine from "./configs/viewEngine"
import initWebRoutes from "./routes/web"
import bodyParser from "body-parser"
import cookieParser from 'cookie-parser'
import session from "express-session"
import flash from "express-flash"
import connectFlash from "connect-flash"
import passport from "passport"
//const expressLayouts = require('express-ejs-layouts')
//const path = require('path')
const app = express()
const sessionStore = new session.MemoryStore //add recently

const accountSid = process.env.TWILIO_ACCOUNT_SID
const authToken = process.env.TWILIO_AUTH_TOKEN
const client = require('twilio')(accountSid, authToken)

client.messages.create({
    body: 'Bonjour et bienvenue sur la plateforme de RFID Campus System...en guise de msg test après création de votre compte',
    from: '+16122559352',
    to: '+221775352211' // on peut opter l'idée selon laquelle chaque utilisateur ait un compte 'twilio'
})
.then(message => console.log(message))


//use cookie parser
app.use(cookieParser('secret'))

//config session
app.use(session({
    secret: 'secret',
    resave: true,
    store: sessionStore, //add recently
    saveUninitialized: false,
    cookie: {
        maxAge: 1000 * 60 * 60 * 24 // 86400000 1 day
    }
}))


app.use(flash())

// Custom flash middleware -- from Ethan Brown's book, 'Web Development with Node & Express'
app.use((req, res, next) =>{
    // if there's a flash message in the session request, make it available in the response, then delete it
    res.locals.sessionFlash = req.session.sessionFlash
    delete req.session.sessionFlash
    next()
})

// Enable body parser post data
app.use(bodyParser.json())
app.use(bodyParser.urlencoded({ extended: true }))

//Config view engine
configViewEngine(app)
//app.use(expressLayouts)
//app.set('views', path.join(__dirname, 'views'))

//Enable flash message
app.use(connectFlash())

//Config passport middleware
app.use(passport.initialize())
app.use(passport.session())

// init all web routes
initWebRoutes(app)

const port = process.env.PORT || 8080
app.listen(port, () => console.log(`L'application est sur le port ${port}!`))