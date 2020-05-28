import React from 'react';
import { Menu, Container, Button } from 'semantic-ui-react';

interface IProps {
  openCreateForm: () => void;
}

const NavBar: React.FC<IProps> = ({ openCreateForm }) => {
  return (
    <Menu fixed="top" inverted>
      <Container>
        <Menu.Item header>
          <img
            src="/Assets/logo.png"
            alt="logo"
            style={{ marginRight: '10px' }}
          />
          Eventify
        </Menu.Item>
        <Menu.Item name="Activities" />
        <Menu.Item>
          <Button
            onClick={() => openCreateForm()}
            positive
            content="Create Activities"
          />
        </Menu.Item>
      </Container>
    </Menu>
  );
};

export default NavBar;
