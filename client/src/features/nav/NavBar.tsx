import React from 'react';
import { Menu, Container } from 'semantic-ui-react';

const NavBar = () => {
  return (
    <Menu fixed="top" inverted>
      <Container>
        <Menu.Item name="home" />
        <Menu.Item name="home" />
        <Menu.Item name="home" />
      </Container>
    </Menu>
  );
};

export default NavBar;
