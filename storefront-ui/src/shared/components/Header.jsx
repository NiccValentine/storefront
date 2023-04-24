import { Dehaze, LocalMall, Store } from '@mui/icons-material';
import { AppBar, IconButton, Menu, MenuItem, Toolbar, Typography } from '@mui/material';
import { Box } from '@mui/system';
import React from 'react';
import { useNavigate } from 'react-router-dom';

const Header = () => {

    const navigate = useNavigate();

    const [navEl, setNavEl] = React.useState(null);
    const navOpen = Boolean(navEl);


    const handleNavClick = (event) => {
        setNavEl(event.currentTarget);
    };
    const handleNavClose = () => {
        setNavEl(null);
    };

    const handleNavStore = () => {
        setNavEl(null);
        navigate("/stores");
    };
    const handleNavProducts = () => {
        setNavEl(null);
        navigate("/products");
    };

    return (
        <React.Fragment>
            <Box sx={{ flexGrow: 1 }}>
                <AppBar
                    position='static'
                >
                    <Toolbar>
                        <IconButton id='basic-button'
                            aria-controls={open ? 'basic-menu' : undefined}
                            aria-haspopup="true"
                            aria-expanded={open ? 'true' : undefined}
                            onClick={handleNavClick}>
                            <Dehaze />
                        </IconButton>
                        <Menu
                            id='basic-menu'
                            navEl={navEl}
                            open={navOpen}
                            onClose={handleNavClose}
                            anchorOrigin={{
                                vertical: 'top',
                                horizontal: 'left',
                            }}
                            transformOrigin={{
                                vertical: 'bottom',
                                horizontal: 'left',
                            }}
                            sx={{
                                marginTop: '35px'
                            }}
                        >
                            <MenuItem onClick={handleNavStore}>
                                <Store /> Stores
                            </MenuItem>
                            <MenuItem onClick={handleNavProducts}>
                                <LocalMall /> Products
                            </MenuItem>
                        </Menu>

                        <Typography variant='h6'>
                            StoreFront
                        </Typography>

                    </Toolbar>
                </AppBar>
            </Box>
        </React.Fragment>
    )
}

export default Header;