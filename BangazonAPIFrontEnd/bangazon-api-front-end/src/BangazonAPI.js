import React, { Component } from "react"
import ApplicationViews from "./components/ApplicationViews"
import Header from "./components/header/Header"
import Footer from "./components/footer/Footer"

class BangazonAPI extends Component {
    render() {
        return <div className="masterContainer">
            <Header/>
            <ApplicationViews/>
            <Footer/>
        </div>
    }
}

export default BangazonAPI