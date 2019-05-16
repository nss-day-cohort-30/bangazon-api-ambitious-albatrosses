import React, { Component } from 'react'
import "./Header.css"


class Header extends Component {
    render() {
        return (
            <div className="header">
                <div className="welcomeToText">Welcome to</div>
                <div><span className="bangazonText">BANGAZON</span><span className="apiText"> API</span></div>
            </div>
        )
    }
}

export default Header