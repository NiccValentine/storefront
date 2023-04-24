import { Add, KeyboardArrowLeft } from '@mui/icons-material';
import { Divider, FormControl, FormGroup, Grid, IconButton, TextField, Toolbar, Typography, useTheme, Box } from '@mui/material';
import React from 'react';
import { useNavigate } from 'react-router-dom';
import StoreCard from '../../../shared/components/StoreCard';
import { v4 as uuid } from 'uuid';
import StoreService from '../../../shared/services/storeService';

const storeService = new StoreService();

const StoreSearch = () => {

    const navigate = useNavigate();
    const theme = useTheme();

    const [stores, setStores] = React.useState([])

    const [searchTerm, setSearchTerm] = React.useState('');

    React.useEffect(() => {
        const timeOutId = setTimeout(() => {
            (() => {
                if (searchTerm !== '') {
                    storeService.search(searchTerm)
                        .then(response => {
                            if (response.status = 200) {
                                setStores(response.data)
                            }
                            else {
                                setStores([])
                            }
                        })
                }
                else {
                    storeService.get().then(response => {
                        setStores(response.data);
                    })
                }
            })();
        }, 500);
        return () => {
            clearTimeout(timeOutId);
        }
    }, [searchTerm]);

    return (
        <React.Fragment>
            <Toolbar>
                <IconButton onClick={() => navigate("/")} color="inherit">
                    <KeyboardArrowLeft fontSize='large' />
                </IconButton>
                <Typography variant='h3'>
                    Store Search
                </Typography>
                <Box sx={{
                    flexGrow: 1
                }} />
                <IconButton onClick={() => navigate(`/stores/${uuid()}/edit`)} color='inherit'>
                    <Add fontSize='large' />
                </IconButton>
            </Toolbar>
            <Divider />
            <FormGroup>
                <FormControl sx={{
                    alignSelf: 'center',
                    justifySelf: 'center',
                    width: '40%',
                    minWidth: '300px',
                    margin: theme.spacing(2)
                }}>
                    <TextField
                        label="Search"
                        value={searchTerm}
                        onChange={(event) => setSearchTerm(event.target.value)}
                    />
                </FormControl>
            </FormGroup>
            <Grid container spacing={3} sx={{
                margin: theme.spacing(1)
            }}>
                {stores.length > '0' && stores.map(store => <StoreCard store={store} url={`/stores/${store.storeId}`} />)}
            </Grid>
        </React.Fragment>
    )
}

export default StoreSearch;