const DBConnection = require ("./../configs/DBConnection")

const achatPage = (req, res) => {
    return res.render('achat')
}

module.exports = {
    achatPage: achatPage
}